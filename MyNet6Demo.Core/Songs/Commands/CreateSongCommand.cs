using AutoMapper;
using FluentValidation;
using MediatR;
using MyNet6Demo.Core.Songs.ViewModels;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Songs.Commands
{
    public class CreateSongCommand : IRequest<SongViewModel>
    {
        public Guid AlbumGuid { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }
    }

    public class CreateSongCommandValidator : AbstractValidator<CreateSongCommand>
    {
        public CreateSongCommandValidator()
        {
            RuleFor(x => x.AlbumGuid)
                .NotEmpty().WithMessage("Missing album guid");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Missing Name");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0");
        }
    }

    public class CreateSongHandler : IRequestHandler<CreateSongCommand, SongViewModel>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public CreateSongHandler(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<SongViewModel> Handle(CreateSongCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _albumRepository.GetByGuidAsync(request.AlbumGuid, cancellationToken);

            if (album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            var song = new Song
            {
                AlbumId = album.Id,
                Name = request.Name,
                Duration = request.Duration
            };

            album.AddSong(song);

            _albumRepository.Update(album);

            await _albumRepository.SaveChangesAsync(cancellationToken);

            var views = _mapper.Map<SongViewModel>(song);

            return views;
        }
    }
}