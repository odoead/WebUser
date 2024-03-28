using E=WebUser.Domain.entities;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Product.extensions
{
    public class ProductRequestParameters : RequestParameters
    {
        public List<E.AttributeName> attributes { get; set; }
        public string SearchTerm;
    }
}
