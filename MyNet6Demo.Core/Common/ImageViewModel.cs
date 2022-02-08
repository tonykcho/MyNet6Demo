namespace MyNet6Demo.Core.Common
{
    public class ImageContent
    {
        public byte[] Image { get; set; }

        public string ContentType { get; set; }

        public ImageContent(byte[] image, string contentType)
        {
            Image = image;
            ContentType = contentType;
        }
    }
}