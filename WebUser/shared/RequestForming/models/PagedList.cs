namespace WebUser.shared.RequestForming.features
{
    public class PagedList<T> : List<T>
    {
        public PagesStat PagesStat { get; }

        public PagedList(IEnumerable<T> items, int totalCount, int pageNumber, int pageSize)
        {
            PagesStat = new PagesStat()
            {
                TotalCount = totalCount,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(totalCount / (double)pageSize),
            };
            AddRange(items);
        }

        public static PagedList<T> PaginateList(IEnumerable<T> source, int totalCount, int pageNumber, int pageSize) =>
            new(source, totalCount, pageNumber, pageSize);
    }
}
