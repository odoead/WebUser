using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Interfaces.IRepository;

namespace Repository.Wrapper
{
    public interface IRepoWrapper
    {
        IBrandRepo BrandRepo { get; }
        IImageRepo ImageRepo { get; }
        IOrderRepo OrderRepo { get; }
        IProductRepo ProductRepo { get; }
        IUserRepo UserRepo { get; }
        ICategoryRepo CategoryRepo { get; }
        public void Save();
    }
}
