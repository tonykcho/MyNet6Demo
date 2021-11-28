using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Domain.Interfaces;
using MyNet6Demo.Domain.Models;

namespace MyNet6Demo.Core.Albums.Queries
{
    public class GetAlbumListQuery : IRequest<PaginatedList<AlbumViewModel>>
    {
        public int PageNumber { get; set; } = 1;

        public int PageSize { get; set; } = 10;

        public string[] Circles { get; set; } = new string[0];
    }

    public class GetAlbumListQueryValidator : AbstractValidator<GetAlbumListQuery>
    {
        public GetAlbumListQueryValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1).WithMessage("PageNumber at least greater than or equal to 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("PageSize at least greater than or equal to 1.");
        }
    }

    public class GetAlbumListHandler : IRequestHandler<GetAlbumListQuery, PaginatedList<AlbumViewModel>>
    {
        private readonly IAlbumRepository _albumRepository;
        private readonly IMapper _mapper;

        public GetAlbumListHandler(IAlbumRepository albumRepository, IMapper mapper)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AlbumViewModel>> Handle(GetAlbumListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            IQueryable<Album> query = _albumRepository.GetQuery()
                .Include(album => album.Songs);

            if(request.Circles.Length > 0)
            {
                query = query.Where(album => request.Circles.Contains(album.Circle));
            }

            int count = await query.CountAsync(cancellationToken);

            var items = await query.Skip((request.PageNumber - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            var views = _mapper.Map<IList<AlbumViewModel>>(items);

            return new PaginatedList<AlbumViewModel>(views, count, request.PageNumber, request.PageSize);
        }
    }
}