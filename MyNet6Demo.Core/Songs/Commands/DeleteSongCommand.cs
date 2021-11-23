using MediatR;
using FluentValidation;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Exceptions;

namespace MyNet6Demo.Core.Songs.Commands
{
    public class DeleteSongCommand : IRequest
    {
        public Guid Guid { get; set; }
    }

    public class DeleteSongCommandValidator : AbstractValidator<DeleteSongCommand>
    {
        public DeleteSongCommandValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing song guid");
        }
    }

    public class DeleteSongHandler : IRequestHandler<DeleteSongCommand>
    {
        private readonly ISongRepository _songRepository;

        public DeleteSongHandler(ISongRepository songRepository)
        {
            _songRepository = songRepository;
        }

        public async Task<Unit> Handle(DeleteSongCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _songRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if(song is null)
            {
                throw new ResourceNotFoundException(nameof(song));
            }

            _songRepository.Remove(song);

            await _songRepository.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}