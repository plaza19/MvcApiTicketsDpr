using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MvcApiTicketsDpr.Models;
using MvcApiTicketsDpr.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Controllers
{
    public class HomeController : Controller
    {

        private ServiceApiTickets service;
        ServiceStorageBlobs serviceblobs;

        public HomeController(ServiceApiTickets service, ServiceStorageBlobs serviceblobs)
        {
            this.service = service;
            this.serviceblobs = serviceblobs;
        }
   

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> VerTickets()
        {
            String token = HttpContext.User.FindFirst(ClaimTypes.Authentication).Value;
            String id = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            List<Ticket> tickets = await this.service.GetTicketsAsync(token, id);
            return View(tickets);
        }

        public IActionResult UploadTicket()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadTicket(String date, String importe, String producto, IFormFile file)
        {
            String baseUri = "https://storagedpr22.blob.core.windows.net/";
            String filename = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value + file.FileName;
            String container = "tickets";
            String url = baseUri + container + "/" + filename;
            await this.serviceblobs.UploadBlobAsync(container, filename, file.OpenReadStream());
            await this.service.UploadBlobTicket(date, importe, filename, url, HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value, producto, HttpContext.User.FindFirst(ClaimTypes.Authentication).Value);



            return View();
        }

    }
}
