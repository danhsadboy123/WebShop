namespace CodeMegaVNPay.Models;

public class ThongTinThanhToan
{
    public string OrderType { get; set; }
    public double Amount { get; set; }
    public string OrderDescription { get; set; }
    public string Name { get; set; }
    public string HoTen { get; set; }
    public string SoDienThoai { get; set; }
    public string Email { get; set; }
    public int ProvinceId { get; set; }
    public int DistrictId { get; set; }
    public int WardId { get; set; }
    public string Address { get; set; }
}