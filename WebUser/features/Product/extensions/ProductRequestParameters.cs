using WebUser.shared.RequestForming.features;

namespace WebUser.features.Product.extensions
{
    public class ProductRequestParameters : RequestParameters
    {
        public string? RequestName { get; set; } = "";
        public List<int> AttributeValueIDs { get; set; } = new List<int>();
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = int.MaxValue;
    }
}
