namespace Backendec.Models
{
    /// <summary>
    /// класс описывающий наличие товара, по нему строилась бд
    /// </summary>

    public class Availability
    {
        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; } // Поле "Availability" в БД
    }
}