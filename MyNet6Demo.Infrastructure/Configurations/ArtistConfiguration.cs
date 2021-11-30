using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Infrastructure.Configurations
{
    public class ArtistConfiguration : IEntityTypeConfiguration<Artist>
    {
        public void Configure(EntityTypeBuilder<Artist> builder)
        {
            builder.ToTable("Artists");
            
            builder.HasKey(artist => artist.Id);
            
            builder.Property(artist => artist.Guid);

            builder.HasIndex(artist => artist.Id);

            builder.Property(artist => artist.Id)
                .ValueGeneratedOnAdd();

            builder.Ignore(artist => artist.DomainEvents);
        }
    }
}