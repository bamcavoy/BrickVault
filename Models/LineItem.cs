using System;
using System.Collections.Generic;

namespace BrickVault.Models;

public partial class LineItem
{
    public int TransactionId { get; set; }

    public byte ProductId { get; set; }

    public byte Qty { get; set; }

    public byte Rating { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual Order Transaction { get; set; } = null!;
}
