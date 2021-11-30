using MediatR;
using FluentValidation;
using MyNet6Demo.Core.Artists.ViewModels;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Exceptions;
using AutoMapper;
using MyNet6Demo.Domain.Models;
using MyNet6Demo.Domain.DomainEvents;

namespace MyNet6Demo.Core.Artists.Commands
{
    public class CreateArtistCommand : IRequest<ArtistViewModel>
    {
        public string Name { get; set; }
    }

    public class CreateArtistCommandValidator : AbstractValidator<CreateArtistCommand>
    {
        public CreateArtistCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Missing artist name");
        }
    }

    public class CreateArtistHandler : IRequestHandler<CreateArtistCommand, ArtistViewModel>
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public CreateArtistHandler(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<ArtistViewModel> Handle(CreateArtistCommand request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Artist artist = await _artistRepository.GetArtistByNameAsync(request.Name, cancellationToken);

            if (artist is not null)
            {
                throw new ResourceAlreadyExistException(request.Name);
            }

            artist = new Artist
            {
                Name = request.Name
            };

            await _artistRepository.AddAsync(artist, cancellationToken);

            artist.DomainEvents.Add(new ArtistCreatedEvent(artist));

            await _artistRepository.SaveChangesAsync(cancellationToken);

            var view = _mapper.Map<ArtistViewModel>(artist);

            return view;
        }
    }
}