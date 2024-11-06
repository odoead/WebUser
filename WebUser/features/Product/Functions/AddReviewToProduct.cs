namespace WebUser.features.Product.Functions;

using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.Exceptions;
using WebUser.features.Review.DTO;
using E = WebUser.Domain.entities;

public class AddReviewToProduct
{
    //input
    public class AddReviewToProductCommand : IRequest<ReviewDTO>
    {
        public string Header { get; set; }
        public string Body { get; set; }
        public int Rating { get; set; }
        public int ProductID { get; set; }
        public string UserId { get; set; } = string.Empty;
    }

    //handler
    public class Handler : IRequestHandler<AddReviewToProductCommand, ReviewDTO>
    {
        private readonly DB_Context dbcontext;
        private readonly UserManager<E.User> userManager;

        public Handler(DB_Context context, UserManager<E.User> userManager)
        {
            dbcontext = context;
            this.userManager = userManager;
        }

        public async Task<ReviewDTO> Handle(AddReviewToProductCommand request, CancellationToken cancellationToken)
        {
            var user = await userManager.FindByEmailAsync(request.UserId);


            var product =
                await dbcontext.Products.FirstOrDefaultAsync(q => q.ID == request.ProductID, cancellationToken: cancellationToken)
                ?? throw new ProductNotFoundException(request.ProductID);
            var Review = new E.Review
            {
                CreateDate = DateTime.UtcNow,
                Body = request.Body,
                Header = request.Header,
                Rating = request.Rating,
                ProductID = request.ProductID,
                Product = product,
                User = user,
                UserID = user.Id,
            };

            await dbcontext.ProductReviews.AddAsync(Review, cancellationToken);
            await dbcontext.SaveChangesAsync(cancellationToken);

            var results = new ReviewDTO
            {
                Body = Review.Body,
                CreateDate = Review.CreateDate,
                Header = Review.Header,
                Rating = Review.Rating,
                UserName = Review.User.FirstName + " " + Review.User.LastName,
            };
            return results;
        }
    }
}
