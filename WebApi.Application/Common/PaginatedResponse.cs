namespace WebApi.Application.Common
{
    public class PaginatedResponse<T>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public IEnumerable<T> Items { get; set; } = [];

        public PaginatedResponse() { }

        public PaginatedResponse(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalItems = count;
            PageNumber = pageNumber;
            PageSize = pageSize;
        }
    }
}