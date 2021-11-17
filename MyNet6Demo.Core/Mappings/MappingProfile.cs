using AutoMapper;
using MyNet6Demo.Core.ViewModels;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Album, AlbumViewModel>();
        }
    }
}