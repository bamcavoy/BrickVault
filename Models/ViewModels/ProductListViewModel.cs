﻿namespace BrickVault.Models.ViewModels
{
    public class ProductListViewModel
    {
        public IQueryable<Product> Products { get; set; }
        public IQueryable<Category> Categories { get; set; }
        public List<string> Colors { get; set; }
        public PaginationInfo PaginationInfo { get; set; } = new PaginationInfo();
        public List<int> SelectedCategories { get; set; }
        public List<string> SelectedColors { get; set; }
        public int ItemsPerPage { get; set; }
    }
}
