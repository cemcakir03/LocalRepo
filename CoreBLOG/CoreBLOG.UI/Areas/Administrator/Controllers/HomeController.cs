using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreBLOG.UI.Areas.Administrator.Controllers
{
    public class HomeController : Controller
    {
        [Area("Administrator"),Authorize]
        public IActionResult Index()
        {
            return View();
        }
    }
}
