namespace Backendec.Models
{
    /// <summary>
    /// ����� ����������� �����������, �� ���� ��������� ��
    /// </summary>

    public class Customer{
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string CardNumber { get; set; } = string.Empty;
    }
}
