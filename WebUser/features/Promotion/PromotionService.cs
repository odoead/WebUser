using WebUser.Data;
using WebUser.features.Promotion.Interfaces;

namespace WebUser.features.Promotion
{
    public class PromotionService : IPromotionService
    {
        private readonly DB_Context dbcontext;

        public PromotionService(DB_Context context)
        {
            dbcontext = context;
        }
    }
}
