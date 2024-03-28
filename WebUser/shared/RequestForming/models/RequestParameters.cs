using WebUser.Domain.entities;

namespace WebUser.shared.RequestForming.features
{
    public abstract class RequestParameters
    {
        public int PageNum { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }
}
