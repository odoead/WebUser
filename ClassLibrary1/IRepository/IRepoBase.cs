using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.IRepository
{
    public interface IReposBase<T>
        where T : class
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> FindByIdAsync(int id);
        Task<bool> CreateAsync(T entity);
        Task<bool> UpdateAsync(T entity);
        Task<bool> DeleteAsync(T entity);
        Task<bool> SaveAsync();
        Task<bool> IsExistsAsync(int id);
    }
}
