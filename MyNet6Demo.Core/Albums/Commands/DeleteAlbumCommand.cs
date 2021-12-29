using FluentValidation;
using MediatR;
using MyNet6Demo.Domain.DomainEvents;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Albums.Commands
{
    public class DeleteAlbumCommand : IRequest
    {
        public Guid Guid { get; set; }
    }

    public class DeleteAlbumCommandValidator : AbstractValidator<DeleteAlbumCommand>
    {
        public DeleteAlbumCommandValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing Guid");
        }
    }

    public class DeleteAlbumHandler : IRequestHandler<DeleteAlbumCommand>
    {
        private readonly IAlbumRepository _albumRepository;

        public DeleteAlbumHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<Unit> Handle(DeleteAlbumCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            _albumRepository.Remove(album);

            album.DomainEvents.Add(new AlbumDeletedEvent(album));

            await _albumRepository.SaveChangesAsync(cancellationToken);

            return default;
        }
    }
}