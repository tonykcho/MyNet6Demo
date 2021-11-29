using MediatR;
using FluentValidation;
using MyNet6Demo.Core.Artists.ViewModels;
using MyNet6Demo.Core.Common;
using AutoMapper;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MyNet6Demo.Core.Artists.Queries
{
    public class GetArtistListQuery : IRequest<PaginatedList<ArtistViewModel>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetArtistListQueryValidator : AbstractValidator<GetArtistListQuery>
    {
        public GetArtistListQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }

    public class GetArtistListHandler : IRequestHandler<GetArtistListQuery, PaginatedList<ArtistViewModel>>
    {
        private readonly IArtistRepository _artistRepository;
        private readonly IMapper _mapper;

        public GetArtistListHandler(IMapper mapper, IArtistRepository artistRepository)
        {
            _mapper = mapper;
            _artistRepository = artistRepository;
        }

        public async Task<PaginatedList<ArtistViewModel>> Handle(GetArtistListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IQueryable<Artist> query = _artistRepository.GetQuery();

            int count = await query.CountAsync(cancellationToken);

            var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            var views = _mapper.Map<IList<ArtistViewModel>>(items);

            return new PaginatedList<ArtistViewModel>(views, count, request.PageNumber, request.PageSize);
        }
    }
}