namespace IIS.FlexberryGisTestStand.Configuration
{
    /// <summary>
    /// Configuration class for uploading files.
    /// </summary>
    public class FileUploaderConfiguration
    {
        /// <summary>
        /// The section name in the configuration file.
        /// </summary>
        public const string SectionName = "FileUploader";

        /// <summary>
        /// The utility for converting XML to JSON.
        /// </summary>
        public string UtilityForConvertXmlToJson { get; set; }

        /// <summary>
        /// The utility for converting geometry data to JSON.
        /// </summary>
        public string UtilityForConvertGeometryDataToJson { get; set; }
    }
}
