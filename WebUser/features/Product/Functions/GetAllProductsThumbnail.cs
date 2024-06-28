namespace WebUser.features.Product.Functions;

using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Product.DTO;
using WebUser.features.Product.Exceptions;
using WebUser.features.Product.extensions;
using WebUser.shared.RequestForming.features;

public class GetAllProductsThumbnail
{
    //input
    public class GetProductByIDRequest : IRequest<ICollection<ProductThumbnailDTO>>
    {
        public RequestParameters Parameters { get; set; }
        public string orderBy { get; set; }
        public string? requestName { get; set; }
        public List<int> attributeValueIDs { get; set; }
        public int minPrice { get; set; }
        public int maxPrice { get; set; }
    }

    //handler
    public class Handler : IRequestHandler<GetProductByIDRequest, ICollection<ProductThumbnailDTO>>
    {
        private readonly IMapper mapper;
        private readonly DB_Context dbcontext;

        public Handler(IMapper mapper, DB_Context context)
        {
            this.mapper = mapper;
            dbcontext = context;
        }

        public async Task<ICollection<ProductThumbnailDTO>> Handle(GetProductByIDRequest request, CancellationToken cancellationToken)
        {
            var data = await dbcontext
                .Products.SearchByName(request.requestName)
                .Sort(request.orderBy)
                .Filter(request.attributeValueIDs, request.minPrice, request.maxPrice)
                .Take(request.Parameters.PageSize)
                .ToListAsync(cancellationToken);
            var results = mapper.Map<ICollection<ProductThumbnailDTO>>(data);
            return PagedList<ProductThumbnailDTO>.PaginateList(results, results.Count, request.Parameters.PageNum, request.Parameters.PageSize);
        }
    }
}
