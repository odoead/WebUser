namespace WebUser.features.Product;

using WebUser.Domain.entities;
using WebUser.shared.Email;

public static class ProductEmailNotification
{
    public static async Task Notify(User user, Product product)
    {
        EmailService emailService = new EmailService();
        var message = new string($"Dear {user.FirstName} your requested product {product.Name}({product.ID}) is again available");
        await emailService.SendEmailAsync(user.Email, "Product arrival", message);

    }
}
