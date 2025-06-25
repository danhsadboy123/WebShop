using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class SystemWeb
{
    public int Id { get; set; }

    public string Server { get; set; }

    public string EmailSend { get; set; }

    public string Name { get; set; }

    public int? Post { get; set; }

    public string EmailSmtp { get; set; }

    public string PassSmtp { get; set; }
}
