using AutoMapper;
using FluentValidation;
using MediatR;
using MyNet6Demo.Core.Songs.ViewModels;
using MyNet6Demo.Domain.Exceptions;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Songs.Queries
{
    public class GetSongByGuidQuery : IRequest<SongViewModel>
    {
        public Guid Guid { get; set; }
    }

    public class GetSongByGuidQueryValidator : AbstractValidator<GetSongByGuidQuery>
    {
        public GetSongByGuidQueryValidator()
        {
            RuleFor(x => x.Guid)
                .NotEmpty().WithMessage("Missing song guid");
        }
    }

    public class GetSongByGuidHandler : IRequestHandler<GetSongByGuidQuery, SongViewModel>
    {
        private readonly ISongRepository _songRepository;

        private readonly IMapper _mapper;

        public GetSongByGuidHandler(ISongRepository songRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _mapper = mapper;
        }

        public async Task<SongViewModel> Handle(GetSongByGuidQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var song = await _songRepository.GetByGuidAsync(request.Guid, cancellationToken);

            if (song is null)
            {
                throw new ResourceNotFoundException(nameof(song));
            }

            return _mapper.Map<SongViewModel>(song);
        }
    }
}