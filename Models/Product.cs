using System;
using System.Collections.Generic;

namespace BrickVault.Models;

public partial class Product
{
    public byte ProductId { get; set; }

    public string Name { get; set; } = null!;

    public short Year { get; set; }

    public int NumParts { get; set; }

    public int Price { get; set; }

    public string ImgLink { get; set; } = null!;

    public string PrimaryColor { get; set; } = null!;

    public string SecondaryColor { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double AvgRating { get; set; }
}
