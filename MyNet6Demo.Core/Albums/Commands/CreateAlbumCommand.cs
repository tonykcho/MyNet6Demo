using AutoMapper;
using FluentValidation;
using MediatR;
using MyNet6Demo.Core.Albums.ViewModels;
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
        }
    }

    public class CreateAlbumHandler : IRequestHandler<CreateAlbumCommand, AlbumViewModel>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public CreateAlbumHandler(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<AlbumViewModel> Handle(CreateAlbumCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Album album = await _albumRepository.GetByAlbumNameAsync(request.AlbumName, cancellationToken);

            if (album is not null)
            {
                throw new ResourceAlreadyExistException(nameof(album));
            }

            album = new Album()
            {
                AlbumName = request.AlbumName,
                Circle = request.Circle,
                ReleaseDate = request.ReleaseDate
            };

            await _albumRepository.UnitOfWork.ExecuteAsync(async () =>
            {
                await _albumRepository.AddAsync(album, cancellationToken);

                await _albumRepository.SaveChangesAsync(cancellationToken);

            }, cancellationToken);

            AlbumViewModel view = _mapper.Map<AlbumViewModel>(album);

            return view;
        }
    }
}