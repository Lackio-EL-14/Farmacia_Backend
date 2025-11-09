

using Farmacia.Core.Response;


namespace Farmacia.Core.Pagination
{
    public class PagedList<T>
    {
        private List<FacturaDtos> items;
        private int total;
        private int page;

        public List<T> Items { get; }
        public int CurrentPage { get; }
        public int PageSize { get; }
        public int TotalCount { get; }
        public int TotalPages { get; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
          
        }

        public PagedList(List<FacturaDtos> items, int total, int page, int pageSize)
        {
            this.items = items;
            this.total = total;
            this.page = page;
            PageSize = pageSize;
        }

        public PaginationMetadata GetMetadata() => new PaginationMetadata
        {
            CurrentPage = CurrentPage,
            PageSize = PageSize,
            TotalCount = TotalCount,
            TotalPages = TotalPages,
            HasNextPage = HasNext,
            HasPreviousPage = HasPrevious
        };

        public static PagedList<T> Create(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            var count = source.Count();
            var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
