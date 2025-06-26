using System;
using System.Collections.Generic;

namespace WebShop.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string Title { get; set; }

    public string TitleEn { get; set; }

    public string Scontents { get; set; }

    public string ScontentsEn { get; set; }

    public string Contents { get; set; }

    public string ContentsEn { get; set; }

    public string Thumb { get; set; }

    public bool Published { get; set; }

    public string Alias { get; set; }

    public string AliasEn { get; set; }

    public DateTime? CreatedDate { get; set; }

    public string Author { get; set; }

    public int? MaTaiKhoan { get; set; }

    public int? PostCatId { get; set; }

    public bool IsHot { get; set; }

    public bool IsNewfeed { get; set; }

    public string TitleSeo { get; set; }

    public string TitleSeoEn { get; set; }

    public string MetaKey { get; set; }

    public string MetaKeyEn { get; set; }

    public string MetaDesc { get; set; }

    public string MetaDescEn { get; set; }

    public int? Views { get; set; }

    public virtual PostCategory PostCat { get; set; }
}
