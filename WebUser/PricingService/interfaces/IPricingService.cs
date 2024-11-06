namespace WebUser.PricingService.interfaces;

using WebUser.Domain.entities;
using WebUser.PricingService.DTO;

/// <summary>
/// Service to calculate cart/order total, product price, applied discounts,promotions
/// </summary>
public interface IPricingService
{
    Task<List<DiscountRecordDTO>> ApplyDiscountAsync(ICollection<int> productIds);
    Task<List<(DiscountRecordDTO record, Coupon coupon)>> ApplyCouponsAsync(ICollection<int> productIds, string couponsCodes);
    Task<(DiscountRecordDTO discount, List<Point> activatedPoints)> ApplyPointsAsync(string userId, int pointsValue);
    Task<(List<DiscountRecordDTO> records, Point? newpoint, List<ProductCalculationDTO> freeitems)> ApplyPromosAsync(
        ICollection<ProductCalculationDTO> productCalculations, User user);

    Task<(CartOrderResultDTO order, Point? newPoint, List<Point> activatedPoints, List<Coupon> activatedCoupons)> GenerateOrderAsync(IEnumerable<(int productId, int quantity)> orderProducts, User user, string codes, int pointsRequested);

}
