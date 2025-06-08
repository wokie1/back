namespace Backendec.Models
{
    /// <summary>
    /// класс описывающий магазины, по нему строилась бд
    /// </summary>

    public class Store{
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
    }
}    