using WebUser.Data;
using WebUser.features.Point.Interfaces;

namespace WebUser.features.Point
{
    public class PointService : IPointService
    {
        private readonly DB_Context _Context;

        public PointService(DB_Context context)
        {
            _Context = context;
        }
    }
}
