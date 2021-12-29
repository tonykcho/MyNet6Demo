using MediatR;
using FluentValidation;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.DomainEvents;

namespace MyNet6Demo.Core.Artists.Commands
{
    public class DeleteArtistCommand : IRequest
    {
        public Guid Guid { get; set; }
    }

    public class DeleteArtistCommandValidator : AbstractValidator<DeleteArtistCommand>
    {
        public DeleteArtistCommandValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing artist guid");
        }
    }

    public class DeleteArtistHandler : IRequestHandler<DeleteArtistCommand>
    {
        private readonly IArtistRepository _artistRepository;

        public DeleteArtistHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<Unit> Handle(DeleteArtistCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var artist = await _artistRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (artist is null)
            {
                throw new ResourceNotFoundException(nameof(artist));
            }

            _artistRepository.Remove(artist);

            artist.DomainEvents.Add(new ArtistDeletedEvent(artist));

            await _artistRepository.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}