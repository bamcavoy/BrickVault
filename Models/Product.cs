using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BrickVault.Models;

public partial class Product
{
    public ICollection<ProductCategory>? ProductCategories { get; set; }
    [Key]
    public byte ProductId { get; set; }


    public string Name { get; set; } = null!;

    public short Year { get; set; }

    public int NumParts { get; set; }

    public int Price { get; set; }

    [StringLength(3500, ErrorMessage = "The URL must be 3500 characters or less.")]
    public string ImgLink { get; set; }

    public string PrimaryColor { get; set; } = null!;

    public string SecondaryColor { get; set; } = null!;

    public string Description { get; set; } = null!;

    public double AvgRating { get; set; }
}
