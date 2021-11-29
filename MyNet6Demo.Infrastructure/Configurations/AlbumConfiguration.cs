using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Infrastructure.Configurations
{
    public class AlbumConfiguration : IEntityTypeConfiguration<Album>
    {
        public void Configure(EntityTypeBuilder<Album> builder)
        {
            builder
                .ToTable("Albums");
                
            builder
                .HasKey(album => album.Id);
                
            builder.Property(album => album.Guid);

            builder
                .Property(album => album.Id)
                .ValueGeneratedOnAdd();

            builder
                .HasIndex(album => album.Id);

            builder
                .Ignore(album => album.DomainEvents);
        }
    }
}