/*using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using WebUser.Data;
using WebUser.features.Promotion.DTO;
using E = WebUser.Domain.entities;
namespace WebUser.features.Promotion.Functions
{
    public class CreatePromotion
    {
        //input
        public class CreatePromotionCommand : IRequest<PromotionDTO>
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public E.Category Category { get; set; }
            public List<E.AttributeName> AttributeNames { get; set; }
            public List<E.PromotionProduct> PromoProducts { get; set; }
            public List<E.Product> products { get; set; }
            public DateTime ActiveFrom { get; set; }
            public DateTime ActiveTo { get; set; }
            public double DiscountVal { get; set; }
            [Range(0.01, 100, ErrorMessage = "Only 0.01-100 range allowed")]
            public float DiscountPercent { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int buyQuantity { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int getQuantity { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int MinPay { get; set; }
            [Range(1, int.MaxValue, ErrorMessage = "Only positive number allowed")]
            public int PointsValue { get; set; }
            [Range(1, 100, ErrorMessage = "Only 0.01-100 range allowed")]
            public int PointsPercent { get; set; }
            public int PointsExpireDays { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreatePromotionCommand, PromotionDTO>
        {
            private IMapper _mapper;
            private DB_Context dbcontext;
            public Handler(DB_Context context, IMapper mapper)
            {
                dbcontext = context;
                _mapper = mapper;
            }
            public async Task<PromotionDTO> Handle(CreatePromotionCommand request, CancellationToken cancellationToken)
            {
                var promotion = new E.Promotion
                {
                    ActiveFrom = request.ActiveFrom,
                    ActiveTo = request.ActiveTo,
                    AttributeNames = request.AttributeNames,
                    Description = request.Description,
                    Category = request.Category,
                    CategoryId = request.Category.ID,
                    CreatedAt = DateTime.UtcNow,
                    DiscountPercent = request.DiscountPercent,
                    DiscountVal = request.DiscountVal,
                    Name = request.Name,
                    buyQuantity = request.buyQuantity,
                    getQuantity = request.getQuantity,
                    MinPay = request.MinPay,
                    PointsExpireDays = request.PointsExpireDays,
                    PointsPercent = request.PointsPercent,
                    PointsValue = request.PointsValue,
                    products = request.products,
                    PromoProducts = request.PromoProducts,
                };
                if (!await dbcontext.promotions.
                    AnyAsync(q => q.ActiveFrom == promotion.ActiveFrom &&
                    q.ActiveTo == promotion.ActiveTo &&
                    q.AttributeNames == promotion.AttributeNames &&
                    q.Description == promotion.Description &&
                    q.Category == promotion.Category &&
                    q.CategoryId == promotion.CategoryId &&
                    q.CreatedAt == promotion.CreatedAt &&
                    q.DiscountPercent == promotion.DiscountPercent &&
                    q.DiscountVal == promotion.DiscountVal &&
                    q.Name == promotion.Name &&
                    q.buyQuantity == promotion.buyQuantity &&
                    q.getQuantity == promotion.getQuantity &&
                    q.MinPay == promotion.MinPay &&
                    q.PointsExpireDays == promotion.PointsExpireDays &&
                    q.PointsPercent == promotion.PointsPercent &&
                    q.PointsValue == promotion.PointsValue &&
                    q.products == promotion.products &&
                    q.PromoProducts == promotion.PromoProducts))
                {
                    await dbcontext.promotions.AddAsync(promotion);
                    await dbcontext.SaveChangesAsync();
                }
                var results = _mapper.Map<PromotionDTO>(promotion);
                return results;
            }
        }
    }
}
*/
