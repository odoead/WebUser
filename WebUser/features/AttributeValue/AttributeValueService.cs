using WebUser.Data;
using WebUser.features.AttributeValue.Interfaces;

namespace WebUser.features.AttributeValue
{
    public class AttributeValueService : IAttributeValueService
    {
        private readonly DB_Context _Context;

        public AttributeValueService(DB_Context context)
        {
            _Context = context;
        }
    }
}
