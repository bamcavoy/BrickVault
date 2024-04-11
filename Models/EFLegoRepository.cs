namespace BrickVault.Models
{
    public class EFLegoRepository : ILegoRepository
    {
        private readonly IntexDbContext _context;
        public EFLegoRepository(IntexDbContext context)
        {
            _context = context;
        }

        public IQueryable<AspNetUser> AspNetUsers => _context.AspNetUsers;


        public IQueryable<Product> Products => _context.Products;
        public IQueryable<Category> Categories => _context.Categories;
        public IQueryable<Customer> Customers { get; }
    }
}