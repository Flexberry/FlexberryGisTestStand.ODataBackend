using System;
using System.Web;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using System.IO;
using ICSSoft.STORMNET;

namespace IIS.FlexberryGisTestStand.controls
{
    public class FileUploaderHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            var FileName = HttpContext.Current.Request.QueryString["FileName"];
            string filePath = Path.GetTempPath();
            string fileTempName = Path.GetTempFileName();
            string rigthFile = "";
            string json = "";
            string fn = Path.GetFileName(FileName.Replace(" ", "_"));

            try
            {
                string fileName = filePath + fn;
                using (var bw = new BinaryWriter(new FileStream(fileName, FileMode.Create, FileAccess.Write)))
                {
                    var buffer = new Byte[32 * 1024];
                    var read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    while (read > 0)
                    {
                        bw.Write(buffer, 0, read);
                        read = context.Request.GetBufferlessInputStream().Read(buffer, 0, buffer.Length);
                    }

                    bw.Close();
                }

                string ext = Path.GetExtension(fileName);
                if (ext == ".zip")
                {
                    DirectoryInfo di = Directory.CreateDirectory(fileTempName.Replace(".tmp", ""));
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

                context.Response.AddHeader("Access-Control-Allow-Origin", "*");
                context.Response.ContentType = "application/json";
                context.Response.Charset = "utf-8";
                context.Response.WriteFile(json);
                context.Response.Flush();

                File.Delete(fileTempName);
                File.Delete(json);
            }
            catch (Exception ex)
            {
                LogService.LogError(ex.Message.ToString());
            }
        }

        /// <summary>
        /// Rosreestr's xml to GeoJson
        /// </summary>
        /// <param name="filename">Input file</param>
        /// <param name="fileTempName">Output file</param>
        /// <returns></returns>
        public string XmlToJson(string filename, string fileTempName)
        {
            string template = HttpContext.Current.Server.MapPath("~/shared/template/");
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
                WorkingDirectory = Path.GetTempPath()
            };
            using (var proc = new Process { StartInfo = procStartInfo })
            {
                proc.Start();
                proc.WaitForExit();

                string result = proc.StandardOutput.ReadToEnd();
                if (result.Length > 0)
                    result += Environment.NewLine;

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
        /// Geometry data to GeoJson
        /// </summary>
        /// <param name="filename">Input file</param>
        /// <param name="fileTempName">Output file</param>
        public string GeomtoGeoJSON(string filename, string fileTempName)
        {
            string command = "-f GeoJSON ";
            command += fileTempName + ".json \"" + filename + "\"" + " -nlt \"MULTIPOLYGON\"";

            var procStarInfo = new ProcessStartInfo("ogr2ogr")
            {
                Arguments = command,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardErrorEncoding = System.Text.Encoding.UTF8,
                StandardOutputEncoding = System.Text.Encoding.UTF8,
                WorkingDirectory = Path.GetTempPath()
            };
            procStarInfo.EnvironmentVariables.Add("GDAL_FILENAME_IS_UTF8", "Off");

            using (var proc = new Process { StartInfo = procStarInfo })
            {
                proc.Start();
                proc.WaitForExit();

                string result = proc.StandardOutput.ReadToEnd();
                if (result.Length > 0)
                    result += Environment.NewLine;

                result += proc.StandardError.ReadToEnd();

                if (!string.IsNullOrEmpty(result))
                    LogService.LogWarn(result);
            }
            return fileTempName + ".json";
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
