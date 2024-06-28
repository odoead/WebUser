namespace WebUser.features.Promotion.DTO
{
    public class PromotionPageDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime ActiveFrom { get; set; }
        public DateTime ActiveTo { get; set; }
        public int HoursLeft { get; set; }
        public int DaysLeft { get; set; }
        public bool IsActive { get; set; }
    }
}
