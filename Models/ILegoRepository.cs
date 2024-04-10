namespace BrickVault.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Category> Categories { get; }
    }
}