using System;

namespace WebShop.Models
{
    public class MauLoi
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
