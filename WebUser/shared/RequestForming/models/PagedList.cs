namespace WebUser.shared.RequestForming.features
{
    public class PagedList<T> : List<T>
    {
        public PagesStat pagesStat { get; }
        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            pagesStat = new PagesStat()
            {
                TotalCount = count,
                CurrentPage = pageNumber,
                PageSize = pageSize,
                PageCount = (int)Math.Ceiling(count / (double)pageSize)
            };
            AddRange(items);
        }
        public static PagedList<T> PaginateList(IEnumerable<T> source, int count, int pageNumber, int pageSize)
        {

           // var count = source.Count();
           // var items = source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return new PagedList<T>(source, count, pageNumber, pageSize);
        }
    }
}
