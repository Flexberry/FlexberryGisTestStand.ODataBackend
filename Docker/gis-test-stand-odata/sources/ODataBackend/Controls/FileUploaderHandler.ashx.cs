namespace IIS.FlexberryGisTestStand.Controls
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
    using OSGeo.OGR;

    public class FileUploaderHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IWebHostEnvironment env;

        public FileUploaderHandlerMiddleware(RequestDelegate next, IWebHostEnvironment env)
        {
            this.next = next;
            this.env = env;
        }

        public async Task Invoke(HttpContext context)
        {
            string FileName = context.Request.Query["FileName"];
            string filePath = Path.GetTempPath();
            string fileTempName = Path.GetTempFileName();
            string rigthFile = string.Empty;
            string json = string.Empty;
            string fn = Path.GetFileName(FileName.Replace(" ", "_"));

            try
            {
                string fileName = filePath + fn;

                using (var bw = new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                {
                    var buffer = new byte[32 * 1024];
                    int read = 0;
                    while ((read = await context.Request.Body.ReadAsync(buffer)) > 0)
                    {
                        bw.Write(buffer, 0, read);
                    }

                    bw.Close();
                }

                string ext = Path.GetExtension(fileName);
                if (ext == ".zip")
                {
                    DirectoryInfo di = Directory.CreateDirectory(fileTempName.Replace(".tmp", string.Empty));
                    FastZipUnpack(fileName, di.FullName);

                    if (Directory.GetFiles(di.FullName, "*.shp").Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(di.FullName, "*.shp").Length;)
                        {
                            rigthFile = Directory.GetFiles(di.FullName, "*.shp")[i];
                            break;
                        }
                    }
                    else if (Directory.GetFiles(di.FullName, "*.tab").Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(di.FullName, "*.tab").Length;)
                        {
                            rigthFile = Directory.GetFiles(di.FullName, "*.tab")[i];
                            break;
                        }
                    }
                    else if (Directory.GetFiles(di.FullName, "*.mif").Length != 0)
                    {
                        for (int i = 0; i < Directory.GetFiles(di.FullName, "*.mif").Length;)
                        {
                            rigthFile = Directory.GetFiles(di.FullName, "*.mif")[i];
                            break;
                        }
                    }

                    json = GeomtoGeoJSON(rigthFile, fileTempName);

                    di.Delete(true);
                    File.Delete(fileName);
                }
                else if (ext == ".xml")
                {
                    json = XmlToJson(fileName, fileTempName);
                    File.Delete(fileName);
                }
                else
                {
                    json = GeomtoGeoJSON(fileName, fileTempName);
                    File.Delete(fileName);
                }

                context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
                context.Response.Headers.ContentType = "application/json; charset=utf-8";

                await context.Response.SendFileAsync(json);

                File.Delete(fileTempName);
                File.Delete(json);
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Rosreestr's xml to GeoJson.
        /// </summary>
        /// <param name="filename">Input file</param>
        /// <param name="fileTempName">Output file</param>
        /// <returns></returns>
        public string XmlToJson(string filename, string fileTempName)
        {
            string filePath = "~/shared/";
            string contentRootPath = this.env.ContentRootPath;
            string template = Path.Combine(contentRootPath, filePath.TrimStart('~', '/'));

            string command = " -o " + fileTempName + ".json " + template + "template.xsl \"" + filename + "\"";
            var procStartInfo = new ProcessStartInfo("/usr/bin/xsltproc")
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

            return fileTempName + ".json";
        }

        public void FastZipUnpack(string zipFileName, string targetDir)
        {
            FastZip fastzip = new FastZip();
            fastzip.ExtractZip(zipFileName, targetDir, null);
        }

        /// <summary>
        /// Geometry data to GeoJson.
        /// </summary>
        /// <param name="filename">Input file</param>
        /// <param name="fileTempName">Output file</param>
        /// <returns></returns>
        public string GeomtoGeoJSON(string filename, string fileTempName)
        {
            // Before use the Gdal classes, we need to configure it.
            GdalConfiguration.ConfigureGdal();
            GdalConfiguration.ConfigureOgr();

            if (string.IsNullOrWhiteSpace(filename) || string.IsNullOrWhiteSpace(fileTempName))
            {
                LogService.LogError("Enter parameter path is not legal!");
            }

            OSGeo.GDAL.Gdal.AllRegister();
            Ogr.RegisterAll();

            // To support Russian path, add this code below.
            OSGeo.GDAL.Gdal.SetConfigOption("GDAL_FILENAME_IS_UTF8", "NO");

            // In order to support Russian in the property table field, add the following sentence.
            OSGeo.GDAL.Gdal.SetConfigOption("SHAPE_ENCODING", string.Empty);

            DataSource ds = Ogr.Open(filename, 0);
            if (ds == null)
            {
                LogService.LogError("fail to open the file!");
            }

            var dv = Ogr.GetDriverByName("GeoJSON");
            if (dv == null)
            {
                LogService.LogError("Open the driver failure!");
            }

            DataSource destinationDS = dv.CopyDataSource(ds, fileTempName + ".json", null);

            // Freeing up resources.
            dv.Dispose();
            ds.Dispose();
            destinationDS.Dispose();

            return fileTempName + ".json";
        }
    }

    public static class FileUploaderHandlerExtensions
    {
        public static IApplicationBuilder UseFileUploaderHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<FileUploaderHandlerMiddleware>();
        }
    }
}
