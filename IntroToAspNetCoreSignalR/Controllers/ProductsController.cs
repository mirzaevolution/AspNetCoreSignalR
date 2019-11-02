using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using IntroToAspNetCoreSignalR.Hubs;
using IntroToAspNetCoreSignalR.Services;
using IntroToAspNetCoreSignalR.Models;
namespace IntroToAspNetCoreSignalR.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductService _productService;
        private readonly IHubContext<NotifyHub,INotifyHub> _notifyHub;
        public ProductsController(IProductService productService, 
            IHubContext<NotifyHub,INotifyHub> notifyHub)
        {
                
            _productService = productService;
            _notifyHub = notifyHub;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> GetAll()
        {
            List<ProductViewModel> list = await _productService.GetAll();
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
                await _productService.Add(product);
                await _notifyHub.Clients.All.ReceiveNotification("Someone has added new data",Status.Add);
                return RedirectToAction("Index");

            }
            return View(product);
        }
        public async Task<IActionResult> Edit(string id)
        {
            ProductViewModel product = await _productService.Get(id);
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
                await _productService.Update(product);
                await _notifyHub.Clients.All.ReceiveUpdateNotification(
                    $"Someone has updated product data, please refresh this page", 
                    Status.Update, token);
                await _notifyHub.Clients.All.ReceiveNotification(
                 $"Someone has updated product data, please refresh this page",
                 Status.Update);
                return RedirectToAction("Index");
            }
            return View(product);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(string ProductId)
        {
            await _productService.Delete(ProductId);
            await _notifyHub.Clients.All.ReceiveNotification($"Someone has deleted a product data, please refresh this page", Status.Delete);

            return RedirectToAction("Index");
        }
    }
}