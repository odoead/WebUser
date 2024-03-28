using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using WebUser.features.Point.Interfaces;
using E = WebUser.Domain.entities;

namespace WebUser.features.Point
{
    public class PointRepo : IPointService
    {
        private DB_Context _Context;
        public PointRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.Point point)
        {

            _Context.points.Add(point);
        }
        public void Delete(E.Point Point)
        {
            _Context.points.Remove(Point);
        }

        public async Task<ICollection<E.Point>>? GetAllAsync()
        {
            return await _Context.points.ToListAsync();
        }

        public async Task<E.Point>? GetByIdAsync(int id)
        {
            return await _Context.points.Where(q => q.ID == id).FirstOrDefaultAsync();
        }

        public async Task<bool> IsExistsAsync(int id)
        {
            return await _Context.points.AnyAsync(q => q.ID == id);
        }
        public void Update(E.Point Point)
        {
            _Context.points.Update(Point);
        }
    }
}
