using WebUser.features.AttributeValue.DTO;

namespace WebUser.features.AttributeName.DTO
{
    public class AttributeNameDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public List<AttributeValueDTO> AttributeValues { get; set; } = new List<AttributeValueDTO>();
        public string? Description { get; set; }
        /*private string normalizedLatinName;
        public string NormalizedLatinName
        {
            get { return normalizedLatinName; }
            set
            {
                var conversion = new Conversion();
                normalizedLatinName = conversion.tola;
            }
        }*/
    }
}
