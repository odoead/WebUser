namespace WebUser.shared.RequestForming.features
{
    public class PagesStat
    {
        public int CurrentPage { get; set; }
        public int PageCount { get; set; }
        public int PageSize { get; set; }
        public int TotalCount { get; set; }
        public bool IsFirst => CurrentPage == 1;
        public bool IsLast => CurrentPage == TotalCount;
        public bool HasNext => CurrentPage < TotalCount;
        public bool HasPrev => CurrentPage > 1;
    }
}
