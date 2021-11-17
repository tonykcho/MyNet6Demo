using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Infrastructure.Configurations
{
    public class SongConfiguration : IEntityTypeConfiguration<Song>
    {
        public void Configure(EntityTypeBuilder<Song> builder)
        {
            builder.HasKey(song => song.Id);

            builder.Property(song => song.Guid);

            builder.Property(song => song.Id)
                .ValueGeneratedOnAdd();

            builder.HasIndex(song => song.Id);

            builder.HasOne(song => song.Album)
                .WithMany(album => album.Songs)
                .HasForeignKey(song => song.AlbumId);
        }
    }
}