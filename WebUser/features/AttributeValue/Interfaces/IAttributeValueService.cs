using WebUser.shared;
using E=WebUser.Domain.entities;
namespace WebUser.features.AttributeValue.Interfaces
{
    public interface IAttributeValueService
    {
        public  Task<E.AttributeValue> GetByIdAsync(ObjectID<E.AttributeValue> Id);
        public  Task<ICollection<E.AttributeValue>> GetAllAsync();
        public void Delete(E.AttributeValue attributeValue);
        public void Update(E.AttributeValue attributeValue);
        public void Create(E.AttributeValue attributeValue);
        public  Task<bool> IsExistsAsync(ObjectID<E.AttributeValue> Id);
    }
}
