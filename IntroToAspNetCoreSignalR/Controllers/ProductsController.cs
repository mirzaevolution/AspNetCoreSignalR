using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using IntroToAspNetCoreSignalR.Hubs;
using IntroToAspNetCoreSignalR.Services;
using IntroToAspNetCoreSignalR.Models;
using IntroToAspNetCoreSignalR.Services.Models;
using AutoMapper;

namespace IntroToAspNetCoreSignalR.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHubContext<NotifyHub,INotifyHub> _notifyHub;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, 
            IHubContext<NotifyHub,INotifyHub> notifyHub,
            IMapper mapper)
        {
                
            _productService = productService;
            _notifyHub = notifyHub;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            List<Product> originalList = await _productService.GetAll();
            List<ProductViewModel> list = _mapper.Map<List<Product>, List<ProductViewModel>>(originalList);
            return Json(new
            {
                data = list
            });
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost] 
        public async Task<IActionResult> Create(ProductViewModel product)
        {
            if (ModelState.IsValid)
            {
                Product originalProduct = _mapper.Map<ProductViewModel, Product>(product);
                await _productService.Add(originalProduct);
                await _notifyHub.Clients.All.ReceiveNotification("Someone has added new data",Status.Add);
                return RedirectToAction("Index");

            }
            return View(product);
        }
        public async Task<IActionResult> Edit(string id)
        {
            Product originalProduct = await _productService.Get(id);
            ProductViewModel product = _mapper.Map<Product, ProductViewModel>(originalProduct);
            ViewBag.Token = Guid.NewGuid().ToString("n");
            if (product == null)
                return RedirectToAction("Index");
            return View(product);
        }
        [HttpPost("{token}")]
        public async Task<IActionResult> Edit(string token, ProductViewModel product)
        { 
            if (ModelState.IsValid)
            {
                Product originalProduct = _mapper.Map<ProductViewModel, Product>(product);
                await _productService.Update(originalProduct);
                await _notifyHub.Clients.All.ReceiveUpdateNotification(
                    $"Someone has updated this product data, please refresh this page",  token, product.Id);
                await _notifyHub.Clients.All.ReceiveNotification(
                 $"Someone has updated product data, please refresh this page",
                 Status.Update);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpPost("Products/Delete/{deleteToken}")]
        public async Task<IActionResult> Delete(string deleteToken, string ProductId)
        {
            await _productService.Delete(ProductId);
            await _notifyHub.Clients.All.ReceiveDeleteNotification
                ($"Someone has deleted this product data, please refresh this page", deleteToken, ProductId);

            return RedirectToAction("Index");
        }
    }
}