using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MvcApiTicketsDpr.Models;
using MvcApiTicketsDpr.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MvcApiTicketsDpr.Controllers
{
    public class ManageController : Controller
    {

        private ServiceApiTickets service;


        public ManageController(ServiceApiTickets service)
        {
            this.service = service;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(String Username, String Password)
        {
            String token = await this.service.GetToken(Username, Password);

            if (token == null)
            {
                ViewBag.mensaje = "Usuario/Password incorrecto";
                return View();
            }
            else
            {
                UsuarioTicket user = await this.service.GetUserId(token);
                HttpContext.Session.SetString("TOKEN", token);
                ClaimsIdentity identity = new ClaimsIdentity(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    ClaimTypes.Name, ClaimTypes.Role);
                identity.AddClaim(new Claim(ClaimTypes.Name, Username));
                identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Idusuario.ToString()));
                identity.AddClaim(new Claim(ClaimTypes.Authentication, token)); 

                ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(30)
                });

                return RedirectToAction("Index", "Home");
            }



        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("TOKEN");
            return RedirectToAction("Index", "Home");
        }
    }
}

