using AutoMapper;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Core.Artists.ViewModels;
using MyNet6Demo.Core.Songs.ViewModels;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Album, AlbumViewModel>()
                .ForMember(album => album.Songs, o => o.MapFrom(src => src.Songs.ToList()));

            CreateMap<Album, AlbumExportRecord>();

            CreateMap<Song, SongViewModel>();

            CreateMap<Song, SongExportRecord>();

            CreateMap<Artist, ArtistViewModel>();
        }
    }
}