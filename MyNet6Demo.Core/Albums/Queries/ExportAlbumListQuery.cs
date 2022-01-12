using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MyNet6Demo.Core.Albums.ViewModels;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Albums.Queries
{
    public class ExportAlbumListQuery : IRequest<CsvViewModel>
    {

    }

    public class ExportAlbumListHandler : IRequestHandler<ExportAlbumListQuery, CsvViewModel>
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

        public async Task<CsvViewModel> Handle(ExportAlbumListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await _albumRepository.GetListAsync(cancellationToken);

            var records = _mapper.Map<IEnumerable<AlbumExportRecord>>(items);

            CsvViewModel csv = new CsvViewModel
            {
                FileName = "Albums.csv",
                ContentType = "text/csv",
                Content = _csvFileBuilder.BuildCsvFile(records)
            };

            return csv;
        }
    }
}