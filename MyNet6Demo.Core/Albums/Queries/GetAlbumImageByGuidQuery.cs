using FluentValidation;
using MediatR;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Core.Interfaces;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Albums.Queries
{
    public class GetAlbumImageByGuidQuery : IRequest<ImageContent>
    {
        public Guid Guid { get; set; }
    }

    public class GetAlbumImageByGuidQueryValidator : AbstractValidator<GetAlbumImageByGuidQuery>
    {
        public GetAlbumImageByGuidQueryValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing album guid");
        }
    }

    public class GetAlbumImageByGuidHandler : IRequestHandler<GetAlbumImageByGuidQuery, ImageContent>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IImageService _imageService;

        public GetAlbumImageByGuidHandler(IAlbumRepository albumRepository, IImageService imageService)
        {
            _albumRepository = albumRepository;
            _imageService = imageService;
        }

        public async Task<ImageContent> Handle(GetAlbumImageByGuidQuery request, CancellationToken cancellationToken)
        {
            var album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if(album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            if(album.ImagePath is null || album.ImageContentType is null)
            {
                throw new ResourceNotFoundException("image");
            }

            var image = await _imageService.DownloadImageAsync(album.ImagePath, cancellationToken);

            return new ImageContent(image, album.ImageContentType);
        }
    }
}