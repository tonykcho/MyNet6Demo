using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Albums.Queries
{
    public class ExportAlbumListQuery : IRequest<AlbumExportViewModel>
    {

    }

    public class ExportAlbumListHandler : IRequestHandler<ExportAlbumListQuery, AlbumExportViewModel>
    {
        private readonly IAlbumRepository _albumRepository;

        private readonly IMapper _mapper;

        private readonly ICsvFileBuilder _csvFileBuilder;

        public ExportAlbumListHandler(IAlbumRepository albumRepository, IMapper mapper, ICsvFileBuilder csvFileBuilder)
        {
            _albumRepository = albumRepository;
            _mapper = mapper;
            _csvFileBuilder = csvFileBuilder;
        }

        public async Task<AlbumExportViewModel> Handle(ExportAlbumListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await _albumRepository.GetQuery().ToListAsync(cancellationToken);

            var records = _mapper.Map<IEnumerable<AlbumExportRecord>>(items);

            AlbumExportViewModel csv = new AlbumExportViewModel
            {
                FileName = "Albums.csv",
                ContentType = "text/csv",
                Content = _csvFileBuilder.BuildCsvFile(records)
            };

            return csv;
        }
    }
}