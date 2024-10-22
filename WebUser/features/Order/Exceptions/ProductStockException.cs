namespace WebUser.features.Order.Exceptions;

public class ProductStockException : Exception
{
    public ProductStockException(int id)
        : base($"Not enought stock for product {id}.") { }
}
