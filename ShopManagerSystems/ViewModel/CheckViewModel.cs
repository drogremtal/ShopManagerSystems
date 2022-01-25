namespace ShopManagerSystems.ViewModel
{
    public class CheckViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Summa { get; set; }
        public IFormFile FileLink { get; set; }
    }
}
