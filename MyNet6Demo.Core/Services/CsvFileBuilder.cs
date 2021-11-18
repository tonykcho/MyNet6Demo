using System.Globalization;
using CsvHelper;
using MyNet6Demo.Domain.Interfaces;

namespace MyNet6Demo.Core.Services
{
    public class CsvFileBuilder : ICsvFileBuilder
    {
        public byte[] BuildCsvFile<T>(IEnumerable<T> records)
        {
            using var memoryStream = new MemoryStream();

            using (var streamWriter = new StreamWriter(memoryStream))
            {
                using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

                csvWriter.WriteRecords(records);
            }

            return memoryStream.ToArray();
        }
    }
}