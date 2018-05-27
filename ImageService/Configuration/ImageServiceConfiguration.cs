using System.Configuration;

namespace ImageService.Configuration
{
    public class ImageConfiguration : IImageConfiguration
    {
        public string[] Handlers { get; set; }
        public string OutputDir { get; set; }
        public string SourceName { get; set; }
        public string LogName { get; set; }
        public string getHandlers()
        {
            return ConfigurationManager.AppSettings["Handler"];
        }

        public int ThumbnailSize { get; set; }
    }
}