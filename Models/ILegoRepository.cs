namespace BrickVault.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
    }
}