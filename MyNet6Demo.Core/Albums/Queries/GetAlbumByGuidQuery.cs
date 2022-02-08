using AutoMapper;
using FluentValidation;
using MediatR;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Albums.Queries
{
    public class GetAlbumByGuidQuery : IRequest<AlbumViewModel>
    {
        public Guid Guid { get; set; }
    }

    public class GetAlbumByGuidQueryValidator : AbstractValidator<GetAlbumByGuidQuery>
    {
        public GetAlbumByGuidQueryValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty()
                .WithMessage("Missing Guid");
        }
    }

    public class GetAlbumByGuidHandler : IRequestHandler<GetAlbumByGuidQuery, AlbumViewModel>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public GetAlbumByGuidHandler(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }
        public async Task<AlbumViewModel> Handle(GetAlbumByGuidQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Album album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            await _albumRepository.Entry(album)
                .Collection(album => album.Songs)
                .LoadAsync();

            AlbumViewModel view = _mapper.Map<AlbumViewModel>(album);

            return view;
        }
    }
}