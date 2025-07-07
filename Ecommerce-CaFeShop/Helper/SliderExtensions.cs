using Ecommerce_CaFeShop.Models;

namespace Ecommerce_CaFeShop.Helper
{
    public static class SliderExtensions
    {
        public static int ThuTu(this Slider slider)
        {
            return slider.ThuTuHienThi ?? 0;
        }

        public static bool IsActive(this Slider slider)
        {
            return slider.TrangThai;
        }
    }
}