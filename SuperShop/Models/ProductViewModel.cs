using Microsoft.AspNetCore.Http;
using SuperShop.Data.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.Policy;

namespace SuperShop.Models
{
    public class ProductViewModel : Product 
    {
        [Display(Name="Image")]
        public IFormFile ImageFile { get; set; }
    }
}
