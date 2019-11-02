using IntroToAspNetCoreSignalR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IntroToAspNetCoreSignalR.Services
{
    public interface IProductService
    {
        Task<bool> Add(ProductViewModel product);
        Task<List<ProductViewModel>> GetAll();
        Task<ProductViewModel> Get(string id);
        Task<bool> Update(ProductViewModel product);
        Task<bool> Delete(string id);
    }
}
