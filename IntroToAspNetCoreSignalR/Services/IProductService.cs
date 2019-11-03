using IntroToAspNetCoreSignalR.Services.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntroToAspNetCoreSignalR.Services
{
    public interface IProductService
    {
        Task<bool> Add(Product product);
        Task<List<Product>> GetAll();
        Task<Product> Get(string id);
        Task<bool> Update(Product product);
        Task<bool> Delete(string id);
    }
}
