using WebUser.features.AttributeValue.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Product.extensions
{
    public class ProductRequestParameters : RequestParameters
    {
        public List<AttributeValueDTO> Attributes { get; set; }
        public string SearchTerm { get; set; }
        public string OrderBy { get; set; }
    }
}
