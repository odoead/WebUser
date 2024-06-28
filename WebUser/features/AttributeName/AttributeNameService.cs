using WebUser.Data;
using WebUser.features.AttributeName.Interfaces;

namespace WebUser.features.AttributeName
{
    public class AttributeNameService : IAttributeNameService
    {
        private readonly DB_Context _Context;

        public AttributeNameService(DB_Context context)
        {
            _Context = context;
        }
    }
}
