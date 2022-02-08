using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Core.Interfaces;
using MyNet6Demo.Domain.DomainEvents;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Albums.Commands
{
    public class CreateAlbumCommand : IRequest<AlbumViewModel>
    {
        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public DateTime ReleaseDate { get; set; }

        public IFormFile Image { get; set; }
    }

    public class CreateAlbumCommandValidator : AbstractValidator<CreateAlbumCommand>
    {
        public CreateAlbumCommandValidator()
        {
            RuleFor(x => x.AlbumName)
                .NotEmpty().WithMessage("Missing album name");

            RuleFor(x => x.Circle)
                .NotEmpty().WithMessage("Missing circle");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty().WithMessage("Missing release date");

            When(x => x.Image is not null, () =>
            {
                RuleFor(x => x.Image)
                    .Must(image => image.ContentType == "image/jpeg" || image.ContentType == "image/jpg" || image.ContentType == "image/png")
                    .WithMessage("Wrong File Type");
            });
        }
    }

    public class CreateAlbumHandler : IRequestHandler<CreateAlbumCommand, AlbumViewModel>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public CreateAlbumHandler(IAlbumRepository albumRepository, IMapper mapper, IImageService imageService)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<AlbumViewModel> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Album album = await _albumRepository.GetByAlbumNameAsync(request.AlbumName, cancellationToken);

            if (album is not null)
            {
                throw new ResourceAlreadyExistException(request.AlbumName);
            }

            string imagePath = null;

            if (request.Image is not null)
            {
                imagePath = await _imageService.UploadImageAsync(request.Image, cancellationToken);
            }

            album = new Album()
            {
                AlbumName = request.AlbumName,
                Circle = request.Circle,
                ReleaseDate = request.ReleaseDate,
                ImagePath = imagePath,
                ImageContentType = imagePath is null ? null : request.Image.ContentType
            };

            await _albumRepository.UnitOfWork.ExecuteAsync(async () =>
            {
                await _albumRepository.AddAsync(album, cancellationToken);

                album.DomainEvents.Add(new AlbumCreatedEvent(album));

                await _albumRepository.SaveChangesAsync(cancellationToken);

            }, cancellationToken);

            AlbumViewModel view = _mapper.Map<AlbumViewModel>(album);

            return view;
        }
    }
}