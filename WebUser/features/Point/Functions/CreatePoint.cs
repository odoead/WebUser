using AutoMapper;
using MediatR;
using E = WebUser.Domain.entities;
using WebUser.shared.RepoWrapper;
using WebUser.features.AttributeValue.DTO;
using WebUser.features.Point.DTO;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WebUser.Domain.entities;

namespace WebUser.features.Point.Functions
{
    public class CreatePoint
    {
        //input
        public class CreatePointCommand : IRequest<PointDTO>
        {
            public int ID { get; set; }
            public int Value { get; set; }
            public bool isExpirable { get; set; }
            public DateTime ExpireDate { get; set; }
            public User User { get; set; }
        }
        //handler
        public class Handler : IRequestHandler<CreatePointCommand, PointDTO>
        {
            private IRepoWrapper _repoWrapper;
            private IMapper _mapper;

            public Handler(IRepoWrapper ServiceWrapper, IMapper mapper)
            {
                _repoWrapper = ServiceWrapper;
                _mapper = mapper;
            }

            public async Task<PointDTO> Handle(CreatePointCommand request, CancellationToken cancellationToken)
            {
                var point = new E.Point
                {
                    CreateDate = DateTime.Now,
                    ExpireDate = request.ExpireDate,
                    User = request.User,
                    ID = request.ID,
                    isExpirable = request.isExpirable,
                    UserId = request.User.Id,
                    Value = request.Value,
                };
                _repoWrapper.Point.Create(point);
                await _repoWrapper.SaveAsync();
                var results = _mapper.Map<PointDTO>(point);
                return results;
            }
        }

    }
}
