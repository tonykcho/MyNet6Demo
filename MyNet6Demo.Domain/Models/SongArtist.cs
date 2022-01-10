using System.Text.Json.Serialization;

namespace MyNet6Demo.Domain.Models
{
    public class SongArtist
    {
        public int ArtistId { get; set; }

        [JsonIgnore]
        public Artist Artist { get; set; }

        public int SongId { get; set; }

        [JsonIgnore]
        public Song Song { get; set; }

        public string ArtistType { get; set; }
    }

    public static class ArtistTypes
    {
        public static string Producer => "Producer";

        public static string Illustrator => "Illustrator";

        public static string Instrumentalist => "Instrumentalist";
    }
}