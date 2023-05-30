namespace IIS.FlexberryGisTestStand.Controllers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSSoft.STORMNET;
    using IIS.FlexberryGisTestStand.Configuration;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Controller for converting spatial files to JSON.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class FileUploaderController : ControllerBase
    {
        private readonly IWebHostEnvironment env;
        private readonly IOptions<FileUploaderConfiguration> configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploaderController"/> class.
        /// </summary>
        /// <param name="env">Information about web hosting environment.</param>
        /// <param name="configuration">An application configuration properties.</param>
        public FileUploaderController(IWebHostEnvironment env, IOptions<FileUploaderConfiguration> configuration)
        {
            this.env = env;
            this.configuration = configuration;
        }

        /// <summary>
        /// A method for processing a file download request and converting the file to JSON.
        /// </summary>
        /// <param name="data">File with geodata.</param>
        /// <returns>Geodata in json format.</returns>
        [HttpPost("convertingSpatialFilesToJson")]
        public async Task<IActionResult> ConvertSpatialFilesToJson(IFormFile data)
        {
            try
            {
                if (data == null)
                {
                    throw new Exception("Data is null");
                }

                string ext = Path.GetExtension(data.FileName);
                string fileName = Path.Join(Path.GetTempPath(), Guid.NewGuid().ToString() + ext);
                string tempFileName = Path.GetTempFileName();
                string rigthFile = string.Empty;
                string fileNameGeoJson = string.Empty;

                using (var stream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
                {
                    await data.CopyToAsync(stream).ConfigureAwait(false);
                }

                if (ext == ".zip")
                {
                    DirectoryInfo directoryInfo = Directory.CreateDirectory(tempFileName.Replace(".tmp", string.Empty, StringComparison.OrdinalIgnoreCase));

                    rigthFile = this.GetGeoFile(fileName, directoryInfo);

                    if (string.IsNullOrEmpty(rigthFile))
                    {
                        directoryInfo.Delete(true);
                        System.IO.File.Delete(fileName);
                        throw new Exception("There are no geometry files in the zip archive.");
                    }

                    fileNameGeoJson = this.GeomToGeoJSON(rigthFile, tempFileName);
                    directoryInfo.Delete(true);
                    System.IO.File.Delete(fileName);
                }
                else if (ext == ".xml")
                {
                    fileNameGeoJson = this.XmlToGeoJson(fileName, tempFileName);
                    System.IO.File.Delete(fileName);
                }
                else
                {
                    fileNameGeoJson = this.GeomToGeoJSON(fileName, tempFileName);
                    System.IO.File.Delete(fileName);
                }

                string[] jsonData = System.IO.File.ReadAllLines(fileNameGeoJson);

                System.IO.File.Delete(tempFileName);
                System.IO.File.Delete(fileNameGeoJson);

                return await Task.FromResult(new JsonResult(jsonData)).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message);
                return new BadRequestObjectResult(ex.Message);
            }
        }

        /// <summary>
        /// Rosreestr's xml to GeoJson.
        /// </summary>
        /// <param name="fileName">Input file.</param>
        /// <param name="tempFileName">Output file.</param>
        /// <returns>The name of the file converted to GeoJson.</returns>
        public string XmlToGeoJson(string fileName, string tempFileName)
        {
            string template = Path.Join(this.env.ContentRootPath, "shared", "template.xls");
            string command = $" -o {tempFileName}.json {template} \"{fileName}\"";
            string utilityForConvertXmlToJson = this.configuration.Value.UtilityForConvertXmlToJson;

            if (string.IsNullOrEmpty(utilityForConvertXmlToJson))
            {
                throw new Exception("The path to the utility is missing in the configuration.");
            }

            if (!System.IO.File.Exists(utilityForConvertXmlToJson))
            {
                throw new Exception($"The utility file is missing at the specified path {utilityForConvertXmlToJson}.");
            }

            var procStartInfo = new ProcessStartInfo(utilityForConvertXmlToJson)
            {
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardErrorEncoding = System.Text.Encoding.UTF8,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                WorkingDirectory = Path.GetTempPath(),
            };

            using (var proc = new Process { StartInfo = procStartInfo })
            {
                proc.Start();
                proc.WaitForExit();

                string result = proc.StandardOutput.ReadToEnd();
                if (result.Length > 0)
                {
                    result += Environment.NewLine;
                }

                result += proc.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(result))
                {
                    LogService.LogWarn(result);
                }
            }

            return tempFileName + ".json";
        }

        /// <summary>
        /// Extract the contents of a zip file.
        /// </summary>
        /// <param name="zipFileName">The zip file to extract from.</param>
        /// <param name="targetDirectory">The directory to save extracted information in.</param>
        public void FastZipUnpack(string zipFileName, string targetDirectory)
        {
            FastZip fastzip = new FastZip();
            fastzip.ExtractZip(zipFileName, targetDirectory, null);
        }

        /// <summary>
        /// Geometry data to GeoJson.
        /// </summary>
        /// <param name="fileName">Input file.</param>
        /// <param name="tempFileName">Output file.</param>
        /// <returns>The name of the file converted to GeoJson.</returns>
        public string GeomToGeoJSON(string fileName, string tempFileName)
        {
            string command = $"-f GeoJSON {tempFileName}.json \"{fileName}\" -nlt \"MULTIPOLYGON\"";
            string utilityForConvertGeometryDataToJson = this.configuration.Value.UtilityForConvertGeometryDataToJson;

            if (string.IsNullOrEmpty(utilityForConvertGeometryDataToJson))
            {
                throw new Exception("The utility name is missing.");
            }

            var procStarInfo = new ProcessStartInfo("ogr2ogr")
            {
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardErrorEncoding = System.Text.Encoding.UTF8,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                WorkingDirectory = Path.GetTempPath(),
            };
            procStarInfo.EnvironmentVariables.Add("GDAL_FILENAME_IS_UTF8", "Off");

            using (var proc = new Process { StartInfo = procStarInfo })
            {
                proc.Start();
                proc.WaitForExit();

                string result = proc.StandardOutput.ReadToEnd();
                if (result.Length > 0)
                {
                    result += Environment.NewLine;
                }

                result += proc.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(result))
                {
                    LogService.LogWarn(result);
                }
            }

            return tempFileName + ".json";
        }

        /// <summary>
        /// Get a geofile.
        /// </summary>
        /// <param name="fileName">Input file.</param>
        /// <param name="directoryInfo">Output temp directory.</param>
        /// <returns>The name of the geofile.</returns>
        private string GetGeoFile(string fileName, DirectoryInfo directoryInfo)
        {
            string rigthFile = string.Empty;
            this.FastZipUnpack(fileName, directoryInfo.FullName);

            string[] shapeExtensionFiles = Directory.GetFiles(directoryInfo.FullName, "*.shp");
            string[] tabExtensionFiles = Directory.GetFiles(directoryInfo.FullName, "*.tab");
            string[] mifExtensionFiles = Directory.GetFiles(directoryInfo.FullName, "*.mif");

            if (shapeExtensionFiles.Length != 0)
            {
                rigthFile = shapeExtensionFiles.First();
            }
            else if (tabExtensionFiles.Length != 0)
            {
                rigthFile = tabExtensionFiles.First();
            }
            else if (mifExtensionFiles.Length != 0)
            {
                rigthFile = mifExtensionFiles.First();
            }

            return rigthFile;
        }
    }
}
