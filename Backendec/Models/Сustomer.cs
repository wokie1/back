namespace Backendec.Models
{
    /// <summary>
    /// класс описывающий покупателей, по нему строилась бд
    /// </summary>

    public class Customer{
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
    }
}
