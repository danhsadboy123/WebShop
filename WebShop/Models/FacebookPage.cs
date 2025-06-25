using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class FacebookPage
{
    public int Id { get; set; }

    public string Userid { get; set; }

    public string TokenAccount { get; set; }

    public string Name { get; set; }

    public string Avatar { get; set; }

    public string NameGroup { get; set; }

    public string AvartarGroup { get; set; }

    public string AppId { get; set; }

    public string GroupId { get; set; }
}
