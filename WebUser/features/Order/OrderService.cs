using WebUser.Data;
using WebUser.features.Order.Interfaces;

namespace WebUser.features.Order
{
    public class OrderService : IOrderService
    {
        private readonly DB_Context dbcontext;

        public OrderService(DB_Context context)
        {
            this.dbcontext = context;
        }
    }
}
