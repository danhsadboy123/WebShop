using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class EmailMaketting
{
    public int EmailId { get; set; }

    public int? AcountId { get; set; }

    public string Title { get; set; }

    public string Header { get; set; }

    public string ContentName { get; set; }

    public string Body { get; set; }

    public DateTime? CreateDate { get; set; }

    public DateTime? CustomDate { get; set; }

    public int? EmailEvent { get; set; }

    public int? Active { get; set; }

    public int? Input { get; set; }

    public virtual Account Acount { get; set; }
}
