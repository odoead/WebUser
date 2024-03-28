using Entities;
using Interfaces.IRepository;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories;
using WebUser.Repository;

namespace Repository.Wrapper
{
    /// <summary>
    /// Repository wrapper - специальный паттерн, который используется
    /// если у вас есть несколько репозиториев и вы хотите предоставить
    /// упрощенный и согласованный интерфейс для доступа к ним.
    /// </summary>
    public class RepositoryWrapper : IRepoWrapper
    {

        public RepositoryWrapper(AppDbContext context)
        {
            _context = context;
        }
        private AppDbContext _context;
        private IBrandRepo _BrandRepo;
        private IImageRepo _ImageRepo;
        private IOrderRepo _OrderRepo;
        private IProductRepo _ProductRepo;
        private IUserRepo _UserRepo;
        private ICategoryRepo _CategoryRepo;
        public IBrandRepo BrandRepo
        {
            get
            {
                if (_BrandRepo == null)
                {
                    _BrandRepo = new BrandRepository(_context);
                }
                return _BrandRepo;
            }
        }

        public IImageRepo ImageRepo
        {
            get
            {
                if (_ImageRepo == null)
                {
                    _ImageRepo = new ImageRepository(_context);

                    ;
                }
                return _ImageRepo;
            }
        }

        public IOrderRepo OrderRepo
        {
            get
            {
                if (_OrderRepo == null)
                {
                    _OrderRepo = new OrderRepository(_context);

                    ;
                }
                return _OrderRepo;
            }
        }


        public IProductRepo ProductRepo
        {
            get
            {
                if (_ProductRepo == null)
                {
                    _ProductRepo = new ProductRepository(_context);

                    ;
                }
                return _ProductRepo;
            }
        }

        public IUserRepo UserRepo
        {
            get
            {
                if (_UserRepo == null)
                {
                    _UserRepo = new UserRepository(_context);

                    ;
                }
                return _UserRepo;
            }
        }

        public ICategoryRepo CategoryRepo
        {
            get
            {
                if (_CategoryRepo == null)
                {
                    _CategoryRepo = new CategoryRepository(_context);

                    ;
                }
                return _CategoryRepo;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}