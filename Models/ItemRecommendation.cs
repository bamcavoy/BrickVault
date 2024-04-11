using System;
using System.Collections.Generic;

namespace BrickVault.Models;

public partial class ItemRecommendation
{
    public byte ProductId { get; set; }

    public string Name { get; set; } = null!;

    public byte Recommendation1 { get; set; }

    public byte Recommendation2 { get; set; }

    public byte Recommendation3 { get; set; }

    public byte Recommendation4 { get; set; }

    public byte Recommendation5 { get; set; }

    public virtual Product Product { get; set; } = null!;
}
