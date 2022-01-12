using AutoMapper;
using MediatR;
using MyNet6Demo.Core.Artists.ViewModels;
using MyNet6Demo.Core.Common;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Artists.Queries
{
    public class ExportArtistListQuery : IRequest<CsvViewModel>
    {

    }

    public class ExportArtistListHandler : IRequestHandler<ExportArtistListQuery, CsvViewModel>
    {
        private readonly IArtistRepository _artistRepository;

        private readonly IMapper _mapper;

        private readonly ICsvFileBuilder _csvFileBuilder;

        public ExportArtistListHandler(IArtistRepository artistRepository, IMapper mapper, ICsvFileBuilder csvFileBuilder)
        {
            _artistRepository = artistRepository;
            _mapper = mapper;
            _csvFileBuilder = csvFileBuilder;
        }

        public async Task<CsvViewModel> Handle(ExportArtistListQuery request, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var items = await _artistRepository.GetListAsync(cancellationToken);

            var records = _mapper.Map<IEnumerable<ArtistExportRecord>>(items);            

            CsvViewModel csv = new CsvViewModel
            {
                FileName = "Artist.csv",
                ContentType = "text/csv",
                Content = _csvFileBuilder.BuildCsvFile(records)
            };

            return csv;
        }
    }
}