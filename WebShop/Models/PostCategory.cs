using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class PostCategory
{
    public int PostCatId { get; set; }

    public string PostCatName { get; set; }

    public DateTime? DateCreated { get; set; }

    public DateTime? DateModified { get; set; }

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
}
