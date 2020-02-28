using System.Collections.Generic;

namespace VendingMachine.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int InventoryQuantity { get; set; }
    }

    public interface IProductsProvider
    {
        List<Product> GetProducts();
        List<int> GetPurchasedProducts();
    }

    public class ProductsProvider : IProductsProvider
    {
        private readonly List<Product> _products;

        public ProductsProvider()
        {
            _products = new List<Product>
            {
                new Product
                {
                    Id = 1,
                    Name = "Coke",
                    Price = 25m,
                    InventoryQuantity = 5
                },
                new Product
                {
                    Id = 2,
                    Name = "Pepsi",
                    Price = 35m,
                    InventoryQuantity = 5
                },
                new Product
                {
                    Id = 3,
                    Name = "Soda",
                    Price = 45m,
                    InventoryQuantity = 5
                },
                new Product
                {
                    Id = 4,
                    Name = "Chocolate Bar",
                    Price = 20.25m,
                    InventoryQuantity = 5
                },
                new Product
                {
                    Id = 5,
                    Name = "Chewing Gum",
                    Price = 10.50m,
                    InventoryQuantity = 5
                },
                new Product
                {
                    Id = 6,
                    Name = "Bottled Water",
                    Price = 15m,
                    InventoryQuantity = 5
                }
            };
        }

        public List<Product> GetProducts()
        {
            return _products;
        }

        public List<int> GetPurchasedProducts()
        {
            return new List<int>
            {
                1,
                2,
                3
            };
        }
    }
}