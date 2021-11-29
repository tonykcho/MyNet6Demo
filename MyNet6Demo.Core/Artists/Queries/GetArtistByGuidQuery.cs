using MediatR;
using FluentValidation;
using MyNet6Demo.Core.Artists.ViewModels;
using MyNet6Demo.Domain.Interfaces;
using AutoMapper;
using MyNet6Demo.Domain.Exceptions;

namespace MyNet6Demo.Core.Artists.Queries
{
    public class GetArtistByGuidQuery : IRequest<ArtistViewModel>
    {
        public Guid Guid { get; set; }
    }

    public class GetArtistByGuidQueryValidator : AbstractValidator<GetArtistByGuidQuery>
    {
        public GetArtistByGuidQueryValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing Artist Guid");
        }
    }

    public class GetArtistByGuidHandler : IRequestHandler<GetArtistByGuidQuery, ArtistViewModel>
    {
        private readonly IArtistRepository _artistRepository;

        private readonly IMapper _mapper;

        public GetArtistByGuidHandler(IArtistRepository artistRepository, IMapper mapper)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
        }

        public async Task<ArtistViewModel> Handle(GetArtistByGuidQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var artist = await _artistRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (artist is null)
            {
                throw new ResourceNotFoundException(nameof(artist));
            }

            var view = _mapper.Map<ArtistViewModel>(artist);

            return view;
        }
    }
}