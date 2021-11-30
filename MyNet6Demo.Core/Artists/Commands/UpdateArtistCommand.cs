using MediatR;
using FluentValidation;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using MyNet6Demo.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Domain.DomainEvents;

namespace MyNet6Demo.Core.Artists.Commands
{
    public class UpdateArtistCommand : IRequest
    {
        public Guid Guid { get; set; }
        public string Name { get; set; }
    }

    public class UpdateArtistCommandValidator : AbstractValidator<UpdateArtistCommand>
    {
        public UpdateArtistCommandValidator()
        {

        }
    }

    public class UpdateArtistHandler : IRequestHandler<UpdateArtistCommand>
    {
        private readonly IArtistRepository _artistRepository;

        public UpdateArtistHandler(IArtistRepository artistRepository)
        {
            _artistRepository = artistRepository;
        }

        public async Task<Unit> Handle(UpdateArtistCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();


            Artist artist = await _artistRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (artist is null)
            {
                throw new ResourceNotFoundException(nameof(artist));
            }

            // Check whether album name is already been used by other album;
            if (await _artistRepository.GetQuery().AnyAsync(artist => artist.Name == request.Name && artist.Guid != artist.Guid) == true)
            {
                throw new ResourceAlreadyExistException(nameof(artist));
            }

            await _artistRepository.UnitOfWork.ExecuteAsync(async () =>
            {
                artist.Name = request.Name;

                _artistRepository.Update(artist);

                // artist.DomainEvents.Add(new AlbumUpdatedEvent(album));
                artist.DomainEvents.Add(new ArtistUpdatedEvent());

                await _artistRepository.SaveChangesAsync(cancellationToken);

            }, cancellationToken);

            return default;
        }
    }
}