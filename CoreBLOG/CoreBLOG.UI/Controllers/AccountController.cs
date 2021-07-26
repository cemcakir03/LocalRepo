using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CoreBLOG.CORE.Service;
using CoreBLOG.MODEL.Entities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

namespace CoreBLOG.UI.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICoreService<User> us;
        public AccountController(ICoreService<User> _us)
        {
            us = _us;
        }
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (us.Any(x =>x.EmailAdress==user.EmailAdress && x.Password==user.Password))
            {
                User logged = us.GetByDefault(x => x.EmailAdress == user.EmailAdress && x.Password == user.Password);

                var claims = new List<Claim>()
                {
                    new Claim("ID",logged.ID.ToString()),
                    new Claim(ClaimTypes.Name,logged.FirstName),
                    new Claim(ClaimTypes.Surname,logged.LastName),
                    new Claim(ClaimTypes.Email,logged.EmailAdress)
                };

                var userIdentity = new ClaimsIdentity(claims, "login");
                var principal = new ClaimsPrincipal(userIdentity);

                await HttpContext.SignInAsync(principal);
                return RedirectToAction("Index", "Home", new { area = "Administrator" });
            }
            return View(user);
        }
    }
}
