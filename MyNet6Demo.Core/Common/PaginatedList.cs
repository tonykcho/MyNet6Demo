using Microsoft.EntityFrameworkCore;

namespace MyNet6Demo.Core.Common
{
    public class PaginatedList<T>
    {
        public IEnumerable<T> Items { get; set; }

        public int PageNumber { get; }

        public int TotalPages { get; }

        public int TotalCount { get; }

        public PaginatedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }
    }
}