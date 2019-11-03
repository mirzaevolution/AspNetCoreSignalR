using AutoMapper;
using IntroToAspNetCoreSignalR.Models;
using IntroToAspNetCoreSignalR.Services.Models;
namespace IntroToAspNetCoreSignalR.MapperConfig
{
    public class MainConfig:Profile
    {
        public MainConfig()
        {
            CreateMap<Product, ProductViewModel>().ReverseMap();
        }
    }
}
