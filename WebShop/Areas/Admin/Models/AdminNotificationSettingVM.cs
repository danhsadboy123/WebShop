using System.Collections.Generic;
using WebShop.Models;

namespace WebShop.Areas.Admin.Models
{
    public class AdminNotificationSettingVM
    {
        public List<EmailMaketting> emails { get; set; }
        public List<CardTemplate> cardTemplates { get; set; }
    }
}
