using Microsoft.EntityFrameworkCore;
using WebUser.Data;
using E=WebUser.Domain.entities;
using WebUser.features.AttributeValue.Interfaces;
using WebUser.Domain.entities;
using WebUser.shared;
using WebUser.features.AttributeName.Exceptions;

namespace WebUser.features.AttributeValue
{
    public class AttributeValueRepo : IAttributeValueService
    {
        private DB_Context _Context;
        public AttributeValueRepo(DB_Context context)
        {
            _Context = context;
        }
        public void Create(E.AttributeValue attributeValue )
        {
            _Context.attributeValues.Add(attributeValue);
        }

        public void Delete(E.AttributeValue attributeValue)
        {
            _Context.attributeValues.Remove(attributeValue);
        }

        public async Task<ICollection<E.AttributeValue>>? GetAllAsync()
        {
            return await _Context.attributeValues.ToListAsync();
        }

        public async Task<E.AttributeValue>? GetByIdAsync(ObjectID<E.AttributeValue>Id)
        {
            return await _Context.attributeValues.Where(q => q.ID == Id.Value).FirstOrDefaultAsync();

        }

        public async Task< bool> IsExistsAsync(ObjectID<E.AttributeValue> Id)
        {
            return await _Context.attributeValues.AnyAsync(q=>q.ID == Id.Value);
        }
        public void Update(E.AttributeValue attributeValue)
        {
            _Context.attributeValues.Update(attributeValue);
        }
    }
}
