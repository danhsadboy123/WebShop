using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models;

public partial class Role
{
    [Key]
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public string Description { get; set; }

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
