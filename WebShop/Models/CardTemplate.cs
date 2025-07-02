using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class CardTemplate
{
    public int CardTemplateId { get; set; }

    public string Title { get; set; }

    public string MoTa { get; set; }

    public string HtmlContent { get; set; }

    public int? Type { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? DateModified { get; set; }
}
