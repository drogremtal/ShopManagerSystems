namespace ShopManagerSystems.ViewModel
{
    public class CheckViewModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public decimal Summa { get; set; }
        public string FileLink { get; set; }
        public IFormFile File { get; set; }
    }
}
