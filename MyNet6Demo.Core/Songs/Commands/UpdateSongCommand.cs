using MediatR;
using FluentValidation;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Exceptions;

namespace MyNet6Demo.Core.Songs.Commands
{
    public class UpdateSongCommand : IRequest
    {
        public Guid SongGuid { get; set; }

        public string Name { get; set; }

        public int Duration { get; set; }
    }

    public class UpdateSongCommandValidator : AbstractValidator<UpdateSongCommand>
    {
        public UpdateSongCommandValidator()
        {
            RuleFor(x => x.SongGuid)
                .NotEmpty().WithMessage("Missing song guid");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Missing name");
            
            RuleFor(x => x.Duration)
                .NotEmpty().WithMessage("Duration must be greater than 0");
        }
    }

    public class UpdateSongHandler : IRequestHandler<UpdateSongCommand>
    {
        private readonly ISongRepository _songRepository;

        public UpdateSongHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<Unit> Handle(UpdateSongCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _songRepository.GetByGuidAsync(request.SongGuid, cancellationToken);

            if(song is null)
            {
                throw new ResourceNotFoundException(nameof(song));
            }

            song.Name = request.Name;
            song.Duration = request.Duration;

            _songRepository.Update(song);

            await _songRepository.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}