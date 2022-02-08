using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyNet6Demo.Core.Interfaces;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Albums.Commands
{
    public class UploadAlbumImageCommand : IRequest<string>
    {
        public Guid Guid { get; set; }
        public IFormFile Image { get; set; }
    }

    public class UploadAlbumImageCommandValidator : AbstractValidator<UploadAlbumImageCommand>
    {
        public UploadAlbumImageCommandValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty()
                .WithMessage("Missing Album Id");

            RuleFor(x => x.Image)
                .NotEmpty()
                .WithMessage("Missing Image");

            When(x => x.Image is not null, () =>
            {
                RuleFor(x => x.Image)
                    .Must(image => image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png")
                    .WithMessage("Wrong file t  ype, only jpg/png are allowed");
            });
        }
    }

    public class UploadAlbumImageHandler : IRequestHandler<UploadAlbumImageCommand, string>
    {
        private readonly IImageService _imageService;

        private readonly IAlbumRepository _albumRepository;

        public UploadAlbumImageHandler(IImageService imageService, IAlbumRepository albumRepository)
        {
            _imageService = imageService;
            _albumRepository = albumRepository;
        }

        public async Task<string> Handle(UploadAlbumImageCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            string imagePath = await _imageService.UploadImageAsync(request.Image, cancellationToken);

            album.ImagePath = imagePath;

            album.ImageContentType = request.Image.ContentType;

            _albumRepository.Update(album);

            await _albumRepository.SaveChangesAsync(cancellationToken);

            return imagePath;
        }
    }
}