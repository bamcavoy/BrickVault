using BrickVault.Controllers.Infrastructure;
using System.Text.Json.Serialization;

namespace BrickVault.Models
{
    public class SessionCart : Cart
    {
        public static Cart GetCart(IServiceProvider services)
        {
            ISession? session = services.GetRequiredService<IHttpContextAccessor>()
                .HttpContext?.Session;

            SessionCart cart = session?.GetJson<SessionCart>("Cart") ??
                new SessionCart();

            cart.Session = session;

            return cart;
        }
        [JsonIgnore]
        public ISession? Session { get; set; }
        public override void AddItem(Product prod, int quantity, int price)
        {
            base.AddItem(prod, quantity, price);
            Session?.SetJson("Cart", this);
        }

        public override void RemoveLine(Product prod)
        {
            base.RemoveLine(prod);
            Session?.SetJson("Cart", this);
        }

        public override void Clear()
        {
            base.Clear();
            Session?.Remove("Cart");
        }
    }
}
