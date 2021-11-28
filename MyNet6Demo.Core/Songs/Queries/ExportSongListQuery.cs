using AutoMapper;
using MediatR;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Core.Songs.ViewModels;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Songs.Queries
{
    public class ExportSongListQuery : IRequest<CsvViewModel>
    {

    }

    public class ExportSongListHandler : IRequestHandler<ExportSongListQuery, CsvViewModel>
    {
        private readonly ISongRepository _songRepository;

        private readonly IMapper _mapper;

        private readonly ICsvFileBuilder _csvFileBuilder;

        public ExportSongListHandler(ISongRepository songRepository, IMapper mapper, ICsvFileBuilder csvFileBuilder)
        {
            _songRepository = songRepository;
            _mapper = mapper;
            _csvFileBuilder = csvFileBuilder;
        }

        public async Task<CsvViewModel> Handle(ExportSongListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await _songRepository.GetListAsync(cancellationToken);

            var records = _mapper.Map<IEnumerable<SongExportRecord>>(items);

            CsvViewModel csv = new CsvViewModel
            {
                FileName = "Songs.csv",
                ContentType = "text/csv",
                Content = _csvFileBuilder.BuildCsvFile(records)
            };

            return csv;
        }
    }
}