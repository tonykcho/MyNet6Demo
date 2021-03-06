namespace MyNet6Demo.Core.Albums.ViewModels
{
    public class AlbumExportRecord
    {
        public Guid Guid { get; set; }

        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string? ImagePath { get; set; }
    }
}