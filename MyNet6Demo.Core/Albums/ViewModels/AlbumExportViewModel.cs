namespace MyNet6Demo.Core.Albums.ViewModels
{
    public class AlbumExportViewModel
    {
        public string FileName { get; set; }

        public string ContentType { get; set; }

        public byte[] Content { get; set; }
    }
}