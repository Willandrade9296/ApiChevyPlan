namespace ApiDatalife.Models
{
    public class StorageSettings
    {

        public string ImageUploadPath { get; set; }
        public string[] AllowedExtensions { get; set; }
        public int MaxFileSizeMB { get; set; }
    }
}
