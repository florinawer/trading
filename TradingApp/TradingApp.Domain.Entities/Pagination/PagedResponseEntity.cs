namespace TradingApp.Domain.Entities.Pagination
{
    public class PagedResponseEntity<T> where T : class
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        //datos
        public T Data { get; set; }
    }
}
