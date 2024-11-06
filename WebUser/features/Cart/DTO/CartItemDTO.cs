namespace WebUser.features.Cart.DTO
{
    public class CartItemDTO
    {
        public int ID { get; set; }
        public int Amount { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public double ProductBasePrice { get; set; }
    }
}
