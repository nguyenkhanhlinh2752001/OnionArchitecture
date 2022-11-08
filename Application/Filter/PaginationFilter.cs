namespace Application.Filter
{
    public class PaginationFilter
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string OrderBy { get; set; }

        public PaginationFilter()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.OrderBy = "CreatedOn desc";
        }

        public PaginationFilter(int pageNumber, int pageSize, string orderby)
        {
            this.PageNumber = pageNumber < 1 ? 1 : pageNumber;
            this.PageSize = pageSize > 10 ? 10 : pageSize;
            this.OrderBy = orderby == null ? "CreatedOn desc" : orderby;
        }
    }
}