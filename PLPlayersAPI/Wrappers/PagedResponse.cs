namespace PLPlayersAPI.Wrappers
{
    public class PagedResponse<T> : List<T>
    {
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public bool HasPrevious { get; set; }
        public bool HasNext { get; set; }

        public PagedResponse(List<T> data, int count, int pageNumber, int pageSize)
        {
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalRecords = count;

            CurrentPage = pageNumber > TotalPages ? TotalPages : pageNumber;
            CurrentPage = CurrentPage < 1 ? 1 : CurrentPage;

            HasPrevious = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;

            AddRange(data.Skip((CurrentPage - 1) * PageSize).Take(PageSize));
        }
    }

}
