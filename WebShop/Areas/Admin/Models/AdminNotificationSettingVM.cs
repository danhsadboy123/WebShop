using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminNotificationSettingVM
    {
        public List<EmailTiepThi> emails { get; set; }
        public List<MauThe> cardTemplates { get; set; }
    }
}
