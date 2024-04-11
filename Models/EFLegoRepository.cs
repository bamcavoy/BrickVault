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

        public void ReseedProductId()
        {
            throw new NotImplementedException();
        }

        public void AddProduct(Product product)
        {
            _context.Products.Add(product);
        }


        public void DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            _context.SaveChanges();
        }



        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}