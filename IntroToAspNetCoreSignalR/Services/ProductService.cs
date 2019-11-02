using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IntroToAspNetCoreSignalR.Models;

namespace IntroToAspNetCoreSignalR.Services
{
    public class ProductService : IProductService
    {
        private static List<ProductViewModel> _products = new List<ProductViewModel>()
        {
            new ProductViewModel
            {
                 Name = "Asus Rog",
                 Description = "Gaming Notebook",
                 Price = 1200
            },
            new ProductViewModel
            {
                Name = "Xiomi Redmi 5A",
                Description = "Smartphone",
                Price = 220
            }
        };
        public Task<bool> Add(ProductViewModel product)
        {
            if (product == null)
                return Task.FromResult(false);
            _products.Add(product);
            return Task.FromResult(true);
        }

        public Task<bool> Delete(string id)
        {
            ProductViewModel product = _products.FirstOrDefault(c => c.Id == id);
            if (product == null)
                return Task.FromResult(false);
            _products.Remove(product);
            return Task.FromResult(true);
        }

        public Task<ProductViewModel> Get(string id)
        {
            ProductViewModel product = _products.FirstOrDefault(c => c.Id == id);

            return Task.FromResult(product);
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            return Task.FromResult(_products);
        }

        public Task<bool> Update(ProductViewModel product)
        {
            if (product == null)
                return Task.FromResult(false);
            ProductViewModel existing = _products.FirstOrDefault(c => c.Id == product.Id);
            existing.Name = product.Name;
            existing.Description = product.Description;
            existing.Price = product.Price;
            return Task.FromResult(true);
        }
    }
}
