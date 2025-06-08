namespace Backendec.Models
{
    /// <summary>
    /// класс описывающий продажи, по нему строилась бд
    /// </summary>
    public class Sale
    {
        public int CustomerId { get; set; }
        public int ProductId { get; set; }
        public DateTime SaleDate { get; set; } // Добавьте, если есть в БД
        public decimal TotalPrice { get; set; } // Добавьте, если нужно
    }
}
 