using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Customer
{
    public int CustomerId { get; set; }

    public string FullName { get; set; }

    public DateTime? Birthday { get; set; }

    public string Avatar { get; set; }

    public string Email { get; set; }

    public string Phone { get; set; }

    public DateTime? CreateDate { get; set; }

    public string Password { get; set; }

    public string Salt { get; set; }

    public DateTime? LastLogin { get; set; }

    public bool Active { get; set; }

    public bool? Gender { get; set; }

    public string Note { get; set; }

    public string CompanyName { get; set; }

    public virtual ICollection<AccountAddress> AccountAddresses { get; set; } = new List<AccountAddress>();

    public virtual ICollection<DiscountAddCustomer> DiscountAddCustomers { get; set; } = new List<DiscountAddCustomer>();

    public virtual ICollection<EmailAttribute> EmailAttributes { get; set; } = new List<EmailAttribute>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();
}
