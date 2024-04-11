namespace BrickVault.Models
{
    public class Cart
    {
        public List<CartLine> Lines { get; set; } = new List<CartLine>();

        // if the item doesnt exist int he cart, add it. If it does exist, update the quantity
        public virtual void AddItem(Product prod, int quantity, int price)
        {
            CartLine? line = Lines
                .Where(x => x.Product.ProductId == prod.ProductId)
                .FirstOrDefault();

            //has the item already been added to the cart?
            if (line == null)
            {
                Lines.Add(new CartLine
                {
                    Product = prod,
                    Quantity = quantity,
                    Price = price
                });
            }
            else
            {
                line.Quantity += quantity;
            }

        }

        public virtual void RemoveLine(Product prod) => Lines.RemoveAll(x => x.Product.ProductId == prod.ProductId);

        public virtual void Clear() => Lines.Clear();

        //public decimal CalculateTotal(int price) => Lines.Sum(x => price * x.Quantity);
        public decimal CalculateTotal() => Lines.Sum(x => x.Price * x.Quantity);

        public class CartLine
        {
            public int CartLineId { get; set; }
            public Product Product { get; set; }
            public int Quantity { get; set; }
            public int Price { get; set; }
        }
    }
}
