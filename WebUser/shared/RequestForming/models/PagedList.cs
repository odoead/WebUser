namespace WebUser.shared.RequestForming.features
{
    public class PagedList<T> : List<T>
    {
        public PagesStat PagesStat { get; }

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            PagesStat = new PagesStat()
            {
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }

        public static PagedList<T> PaginateList(IEnumerable<T> source, int count, int pageNumber, int pageSize) =>
            new PagedList<T>(source, count, pageNumber, pageSize);
    }
}
