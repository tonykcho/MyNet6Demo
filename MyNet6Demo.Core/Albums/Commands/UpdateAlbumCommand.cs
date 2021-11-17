using FluentValidation;
using MediatR;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Albums.Commands
{
    public class UpdateAlbumCommand : IRequest
    {
        public Guid Guid { get; set; }

        public string AlbumName { get; set; }

        public string Circle { get; set; }

        public DateTime ReleaseDate { get; set; }
    }

    public class UpdateAlbumCommandValidator : AbstractValidator<UpdateAlbumCommand>
    {
        public UpdateAlbumCommandValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty()
                .WithMessage("Missing Guid");

            RuleFor(x => x.AlbumName)
                .NotEmpty()
                .WithMessage("Missing album name");

            RuleFor(x => x.Circle)
                .NotEmpty()
                .WithMessage("Missing circle");

            RuleFor(x => x.ReleaseDate)
                .NotEmpty()
                .WithMessage("Missing ReleaseDate");
        }
    }

    public class UpdateAlbumHandler : IRequestHandler<UpdateAlbumCommand>
    {
        private readonly IAlbumRepository _albumRepository;

        public UpdateAlbumHandler(IAlbumRepository albumRepository)
        {
            _albumRepository = albumRepository;
        }

        public async Task<Unit> Handle(UpdateAlbumCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Album album = await _albumRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (album is null)
            {
                throw new ResourceNotFoundException(nameof(album));
            }

            // Check whether album name is already been used by other album;
            Album a = await _albumRepository.GetByAlbumNameAsync(request.AlbumName, cancellationToken);

            if (a is not null && a.Guid == album.Guid)
            {
                throw new ResourceAlreadyExistException(nameof(album));
            }

            await _albumRepository.UnitOfWork.ExecuteAsync(async () =>
            {

                album.AlbumName = request.AlbumName;

                album.Circle = request.Circle;

                album.ReleaseDate = request.ReleaseDate;

                _albumRepository.Update(album);

                await _albumRepository.SaveChangesAsync(cancellationToken);

            }, cancellationToken);

            return default;
        }
    }
}