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
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count/(double)pageSize);
            TotalRecords = count;

            HasPrevious = CurrentPage > 1;
            HasNext = CurrentPage < TotalPages;

            AddRange(data);
        }

        public static PagedResponse<T> ToPagedResponse(List<T> data, int pageNumber, int pageSize)
        {
            var count = data.Count();
            var items = data.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return new PagedResponse<T>(items, count, pageNumber, pageSize);
        }

    }
}
