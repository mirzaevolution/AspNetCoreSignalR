using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntroToAspNetCoreSignalR.Models;
using IntroToAspNetCoreSignalR.Services.Models;

namespace IntroToAspNetCoreSignalR.Services
{
    public class ProductService : IProductService
    {
        private static List<Product> _products = new List<Product>()
        {
            new Product
            {
                 Name = "Asus Rog",
                 Description = "Gaming Notebook",
                 Price = 1200
            },
            new Product
            {
                Name = "Xiomi Redmi 5A",
                Description = "Smartphone",
                Price = 220
            }
        };
        public Task<bool> Add(Product product)
        {
            if (product == null)
                return Task.FromResult(false);
            _products.Add(product);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(string id)
        {
            Product product = _products.FirstOrDefault(c => c.Id == id);
            if (product == null)
                return Task.FromResult(false);
            _products.Remove(product);
            return Task.FromResult(true);
        }

        public Task<Product> Get(string id)
        {
            Product product = _products.FirstOrDefault(c => c.Id == id);

            return Task.FromResult(product);
        }

        public Task<List<Product>> GetAll()
        {
            return Task.FromResult(_products);
        }

        public Task<bool> Update(Product product)
        {
            if (product == null)
                return Task.FromResult(false);
            Product existing = _products.FirstOrDefault(c => c.Id == product.Id);
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            return Task.FromResult(true);
        }
    }
}
