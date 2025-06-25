using System;
using System.ComponentModel.DataAnnotations;

namespace WebShop.ModelViews
{
    public class ChangeInfoViewModel
    {
        [Key]
        public int CustomerId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime Birthday { get; set; }
        public bool Gender { get; set; }
    }
}
