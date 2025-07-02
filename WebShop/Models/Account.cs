using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models;

public partial class Account
{
    [Key]
    public int AccountId { get; set; }

    public string PhoneNumber { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public bool IsActivated { get; set; }

    public string FullName { get; set; }

    public int? RoleId { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreatedAt { get; set; }

    //public virtual ICollection<EmailMarketing> EmailMarketings { get; set; } = new List<EmailMarketing>();

    public virtual Role Role { get; set; }
}
