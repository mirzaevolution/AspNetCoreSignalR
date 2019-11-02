using System;
using System.ComponentModel.DataAnnotations;

namespace IntroToAspNetCoreSignalR.Models
{
    public class ProductViewModel
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public decimal Price { get; set; }

    }
}
