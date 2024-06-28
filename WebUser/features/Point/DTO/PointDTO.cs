namespace WebUser.features.Point.DTO
{
    public class PointDTO
    {
        public int ID { get; set; }
        public int Value { get; set; }
        public int BalanceLeft { get; set; }
        public bool isExpirable { get; set; }
        public bool IsUsed { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpireDate { get; set; }
        public string UserID { get; set; }
        public int? OrderID { get; set; } = null;
    }
}
