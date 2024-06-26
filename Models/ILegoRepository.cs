namespace BrickVault.Models
{
    public interface ILegoRepository
    {
        public IQueryable<Product> Products { get; }
        public IQueryable<Category> Categories { get; }
        public  IQueryable<Customer> Customers { get; }
        public  IQueryable<AspNetUser> AspNetUsers { get;  }

        public IQueryable<ItemRecommendation> ItemRecommendations { get; }   
        
        public IQueryable<Order> Orders { get; }

        void SaveChanges();
        void AddProduct(Product product);

        void DeleteProduct(Product product);
        void UpdateProduct(Product product);
        void AddProductCategory(ProductCategory productCategory);

    }
}