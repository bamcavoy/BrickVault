using System;
using System.Collections.Generic;

namespace BrickVault.Models;

public partial class Category
{
    public ICollection<ProductCategory> ProductCategories { get; set; }
    public byte CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;
}
