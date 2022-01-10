using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Core.Songs.ViewModels;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Songs.Queries
{
    public class GetSongListQuery : IRequest<PaginatedList<SongViewModel>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;
    }

    public class GetSongListQueryValidator : AbstractValidator<GetSongListQuery>
    {
        public GetSongListQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("Page Number at least greater than or equal to 1.");
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page Number at least greater than or equal to 1.");
        }
    }

    public class GetSongListHandler : IRequestHandler<GetSongListQuery, PaginatedList<SongViewModel>>
    {
        private readonly ISongRepository _songRepository;

        private readonly IMapper _mapper;

        public GetSongListHandler(ISongRepository songRepository, IMapper mapper)
        {
            _songRepository = songRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<SongViewModel>> Handle(GetSongListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IQueryable<Song> query = _songRepository.GetQuery();

            int count = await query.CountAsync(cancellationToken);

            var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync(cancellationToken);

            var views = _mapper.Map<IList<SongViewModel>>(items);

            return new PaginatedList<SongViewModel>(views, count, request.PageNumber, request.PageSize);
        }
    }
}