namespace IIS.FlexberryGisTestStand.Controllers
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using ICSharpCode.SharpZipLib.Zip;
    using ICSSoft.STORMNET;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Middleware class for converting spatial files to JSON.
    /// </summary>
    public class FileUploaderHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;
        private readonly IConfiguration configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileUploaderHandlerMiddleware"/> class.
        /// </summary>
        /// <param name="next">A delegate that allows passing the request further along the pipeline for processing by other handlers or components.</param>
        /// <param name="env">Information about web hosting environment.</param>
        /// <param name="configuration">An application configuration properties.</param>
        public FileUploaderHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env, IConfiguration configuration)
        {
            this.next = next;
            this.env = env;
            this.configuration = configuration;
        }

        /// <summary>
        /// Invokes the middleware to handle the file upload request and convert the file to JSON.
        /// </summary>
        /// <param name="context">Encapsulates all information about a separate HTTP request and response.</param>
        /// <returns>A <see cref="Task"/> representing the result of the asynchronous operation.</returns>
        public async Task Invoke(HttpContext context)
        {
            if (context == null)
            {
                LogService.LogError("HttpContext is null.");
                return;
            }

            string fileName = context.Request.Query["FileName"];
            string normalizedFileName = Path.GetFileName(fileName.Replace(" ", "_", StringComparison.InvariantCulture));
            string tempPath = Path.GetTempPath();
            string tempFileName = Path.GetTempFileName();
            fileName = Path.Join(tempPath, normalizedFileName);

            string rigthFile = string.Empty;
            string fileNameGeoJson = string.Empty;

            try
            {
                using (var binaryWriter = new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                {
                    var buffer = new byte[32 * 1024];
                    int read = 0;
                    while ((read = await context.Request.Body.ReadAsync(buffer).ConfigureAwait(false)) > 0)
                    {
                        binaryWriter.Write(buffer, 0, read);
                    }

                    binaryWriter.Close();
                }

                string ext = Path.GetExtension(fileName);
                if (ext == ".zip")
                {
                    const string shapeFileExtension = "*.shp";
                    const string tabFileExtension = "*.tab";
                    const string mifFileExtension = "*.mif";

                    DirectoryInfo directoryInfo = Directory.CreateDirectory(tempFileName.Replace(".tmp", string.Empty, StringComparison.OrdinalIgnoreCase));
                    this.FastZipUnpack(fileName, directoryInfo.FullName);

                    if (Directory.GetFiles(directoryInfo.FullName, shapeFileExtension).Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(directoryInfo.FullName, shapeFileExtension).Length;)
                        {
                            rigthFile = Directory.GetFiles(directoryInfo.FullName, shapeFileExtension)[i];
                            break;
                        }
                    }
                    else if (Directory.GetFiles(directoryInfo.FullName, tabFileExtension).Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(directoryInfo.FullName, tabFileExtension).Length;)
                        {
                            rigthFile = Directory.GetFiles(directoryInfo.FullName, tabFileExtension)[i];
                            break;
                        }
                    }
                    else if (Directory.GetFiles(directoryInfo.FullName, mifFileExtension).Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(directoryInfo.FullName, mifFileExtension).Length;)
                        {
                            rigthFile = Directory.GetFiles(directoryInfo.FullName, mifFileExtension)[i];
                            break;
                        }
                    }

                    fileNameGeoJson = this.GeomtoGeoJSON(rigthFile, tempFileName);

                    directoryInfo.Delete(true);
                    File.Delete(fileName);
                }
                else if (ext == ".xml")
                {
                    fileNameGeoJson = this.XmlToGeoJson(fileName, tempFileName);
                    File.Delete(fileName);
                }
                else
                {
                    fileNameGeoJson = this.GeomtoGeoJSON(fileName, tempFileName);
                    File.Delete(fileName);
                }

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.ContentType = "application/json; charset=utf-8";

                await context.Response.SendFileAsync(fileNameGeoJson).ConfigureAwait(false);

                File.Delete(tempFileName);
                File.Delete(fileNameGeoJson);
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message);
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
            string utilityForConvertXmlToJson = this.configuration["UtilityForConvertXmlToJson"];

            if (string.IsNullOrEmpty(utilityForConvertXmlToJson))
            {
                LogService.LogError("The path to the utility is missing in the configuration.");
                return string.Empty;
            }
            else if (!File.Exists(utilityForConvertXmlToJson))
            {
                LogService.LogError("The utility file is missing at the specified path.");
                return string.Empty;
            }
            else
            {
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
        public string GeomtoGeoJSON(string fileName, string tempFileName)
        {
            string command = $"-f GeoJSON {tempFileName}.json \"{fileName}\" -nlt \"MULTIPOLYGON\"";
            string utilityForConvertGeometryDataToJson = this.configuration["UtilityForConvertGeometryDataToJson"];

            if (string.IsNullOrEmpty(utilityForConvertGeometryDataToJson))
            {
                LogService.LogError("The utility name is missing.");
                return string.Empty;
            }
            else
            {
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
            }

            return tempFileName + ".json";
        }
    }

    /// <summary>
    /// Provides extension methods for the <see cref="IApplicationBuilder"/> interface to enable easy registration of the <see cref="FileUploaderHandlerMiddleware"/>.
    /// </summary>
    public static class FileUploaderHandlerMiddlewareExtensions
    {
        /// <summary>
        /// Adds the <see cref="FileUploaderHandlerMiddleware"/> to the request pipeline.
        /// </summary>
        /// <param name="builder">The <see cref="IApplicationBuilder"/> instance.</param>
        /// <returns>The <see cref="IApplicationBuilder"/> instance with the <see cref="FileUploaderHandlerMiddleware"/> added.</returns>
        public static IApplicationBuilder UseFileUploaderHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileUploaderHandlerMiddleware>();
        }
    }
}
