using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using tm_mvc.Models;

namespace tm_mvc.Controllers
{
    public class ProductEditController : Controller
    {

        public IActionResult Index(ProductDto item)
        {
            return View(item);
        }

        [ActionName("ProductEdit")]
        [HttpPost]
        public IActionResult ProductEdit(ProductDto item, IFormFile Image)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Program.api_url;
                if (Image != null)
                {
                    using (var target = new MemoryStream())
                    {
                        Image.CopyTo(target);
                        item.Photo = target.ToArray();
                    }
                }
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");
                HttpResponseMessage response;
                if (item.Id <= 0)
                    response = client.PostAsync(Program.api_route, httpContent).Result;
                else
                    response = client.PutAsync($"{Program.api_route}/{item.Id}", httpContent).Result;
                if (response.IsSuccessStatusCode)
                    ViewBag.result = "New product submited successfully";
                else if (response.StatusCode == System.Net.HttpStatusCode.UnprocessableEntity)
                    ViewBag.result = "Product Code already exists";
                else
                    ViewBag.result = "Error while submitting product";
            }

            return View("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}