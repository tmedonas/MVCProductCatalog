using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeOpenXml;
using tm_mvc.Models;


namespace tm_mvc.Controllers
{
    public class HomeController : Controller
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public HomeController(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet(Name = "Home")]
        public IActionResult Index()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Program.api_url;
                HttpResponseMessage response = client.GetAsync(Program.api_route).Result;
                if (response.IsSuccessStatusCode)
                    ViewBag.result = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result.OrderBy(o => o.Id);
                
            }
            return View();
        }

        [ActionName("Delete")]
        [HttpGet]
        public IActionResult DeleteProduct(long Id)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = Program.api_url;
                HttpResponseMessage response = client.DeleteAsync($"{Program.api_route}/{Id}").Result;
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("Export")]
        public IActionResult Export()
        {
            List<ProductDto> items = new List<ProductDto>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = Program.api_url;
                HttpResponseMessage response = client.GetAsync(Program.api_route).Result;
                if (response.IsSuccessStatusCode)
                    items = response.Content.ReadAsAsync<IEnumerable<ProductDto>>().Result.OrderBy(o => o.Id).ToList();

            }
            if (items.Count == 0)
                return RedirectToAction("Index");
            string sWebRootFolder = _hostingEnvironment.WebRootPath; 
            string sFileName = @"ProductCatalog.xlsx";
            string URL = string.Format("{0}://{1}/{2}", Request.Scheme, Request.Host, sFileName);
            FileInfo file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            if (file.Exists)
            {
                file.Delete();
                file = new FileInfo(Path.Combine(sWebRootFolder, sFileName));
            }
            using (ExcelPackage package = new ExcelPackage(file))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("ProductCatalog");
                //First add the headers
                int h_counter = 0;
                foreach (var h in items.First()?.GetType()?.GetProperties())
                {
                    h_counter++;
                    worksheet.Cells[1, h_counter].Value = h.Name;
                }

                //Add values
                int d_counter = 1;
                foreach (var d in items)
                {
                    d_counter++;
                    h_counter = 0;
                    foreach (var h in d.GetType()?.GetProperties())
                    {
                        h_counter++;
                        var value = h.GetValue(d, null);
                        worksheet.Cells[d_counter, h_counter].Value = value?.ToString();
                    }
                }
                package.Save(); //Save the workbook.
            }
            return Redirect(URL);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
