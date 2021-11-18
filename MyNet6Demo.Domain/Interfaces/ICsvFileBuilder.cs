using System.Globalization;
using CsvHelper;

namespace MyNet6Demo.Domain.Interfaces
{
    public interface ICsvFileBuilder
    {
        byte[] BuildCsvFile<T>(IEnumerable<T> records);
    }
}