namespace SP.Infrastructure.Models
{
    public class GridPagingViewModel
    {
        public GridPagingViewModel() { }

        public GridPagingViewModel(int pageIndex, int pageSize, string sort, string sortDirection = "DESC", int rowsCount = 0, string refreshUrl = "")
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Sort = sort;
            SortDirection = sortDirection;
            RowsCount = rowsCount;
            RefreshUrl = refreshUrl;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int RowsCount { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public string RefreshUrl { get; set; }
    }
}
