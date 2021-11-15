using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Infrastructure.Configurations
{
    public class SongArtistConfiguration : IEntityTypeConfiguration<SongArtist>
    {
        public void Configure(EntityTypeBuilder<SongArtist> builder)
        {
            builder.HasKey(songArtist => new { songArtist.ArtistId, songArtist.SongId });

            builder.HasIndex(songArtist => songArtist.ArtistId);

            builder.HasIndex(songArtist => songArtist.SongId);

            builder.HasOne(songArtist => songArtist.Artist)
                .WithMany(artist => artist.SongArtists)
                .HasForeignKey(songArtist => songArtist.ArtistId);

            builder.HasOne(songArtist => songArtist.Song)
                .WithMany(song => song.SongArtists)
                .HasForeignKey(songArtist => songArtist.SongId);
        }
    }
}