using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Cart.DTO;
using WebUser.shared.RequestForming.features;

namespace WebUser.features.Cart.functions
{
    public class GetAllCarts
    {
        //input
        public class GetAllCartsQuery : IRequest<PagedList<CartDTO>>
        {
            public GetAllCartsQuery(CartRequestParameters parameters)
            {
                this.Parameters = parameters;
            }

            public CartRequestParameters Parameters { get; set; }
        }

        //handler
        public class Handler : IRequestHandler<GetAllCartsQuery, PagedList<CartDTO>>
        {
            private readonly DB_Context dbcontext;

            public Handler(DB_Context context)
            {
                dbcontext = context;
            }

            public async Task<PagedList<CartDTO>> Handle(GetAllCartsQuery request, CancellationToken cancellationToken)
            {
                var data = dbcontext.Carts.Include(q => q.Items).ThenInclude(q => q.Product).AsQueryable();
                var src = await data.Skip((request.Parameters.PageNumber - 1) * request.Parameters.PageSize)
                    .Take(request.Parameters.PageSize)
                    .ToListAsync(cancellationToken);
                var dto = new List<CartDTO>();
                foreach (var cart in src)
                {
                    var cartDto = new CartDTO
                    {
                        ID = cart.ID,
                        UserId = cart.UserID,
                        Items = new List<CartItemDTO>(),
                    };
                    foreach (var item in cart.Items)
                    {
                        cartDto.Items.Add(
                            new CartItemDTO
                            {
                                Amount = item.Amount,
                                ID = item.ID,
                                ProductBasePrice = item.Product.Price,
                                ProductID = item.ProductID,
                                ProductName = item.Product.Name,
                            }
                        );
                    }
                    dto.Add(cartDto);
                }
                var totalCount = await data.CountAsync(cancellationToken);
                var pagedList = PagedList<CartDTO>.PaginateList(
                    source: dto,
                    totalCount: totalCount,
                    pageNumber: request.Parameters.PageNumber,
                    pageSize: request.Parameters.PageSize
                );

                return pagedList;
            }
        }
    }
}
