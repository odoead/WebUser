using WebUser.features.Product.DTO;

namespace WebUser.features.OrderProduct.DTO
{
    public class OrderProductDTO
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public ProductMinDTO Product { get; set; }
        public double FinalPrice { get; set; }
        public double TotalFinalPrice { get; set; }
    }
}
