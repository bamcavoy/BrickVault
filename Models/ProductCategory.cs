using System;
using System.Collections.Generic;

namespace BrickVault.Models;

public partial class ProductCategory
{
    public byte ProductId { get; set; }

    public byte CategoryId { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
