namespace BrickVault.Models
{
    public class EFLegoRepository : ILegoRepository
    {
        private IntexDbContext _context;
        public EFLegoRepository(IntexDbContext temp) {
            _context = temp;
        }

        public IQueryable<Product> Products => _context.Products;

    }
}