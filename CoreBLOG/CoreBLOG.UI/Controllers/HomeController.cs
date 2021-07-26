using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoreBLOG.UI.Models;
using CoreBLOG.CORE.Service;
using CoreBLOG.MODEL.Entities;

namespace CoreBLOG.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICoreService<Category> cs;
        private readonly ICoreService<Post> ps;
        private readonly ICoreService<User> us;
        private readonly ICoreService<Comment> cms;

        public HomeController(ICoreService<Category> _cs, ICoreService<Post> _ps, ICoreService<User> _us, ICoreService<Comment> _cms)
        {
            cs = _cs;
            ps = _ps;
            us = _us;
            cms = _cms;
        }

        public IActionResult Index()
        {
            ViewBag.Categories = cs.GetActive();
            return View(ps.GetActive());
        }

        public IActionResult PostByCategoryID(Guid id)
        {
            ViewBag.Categories = cs.GetActive();
            return View(ps.GetDefault(x => x.CategoryID == id).ToList());
        }

        public IActionResult Post(Guid id)
        {
            //Verilen idye göre Post'u bul, görüntülenmesini 1 artır ve veritabanına kaydet.
            Post post = ps.GetByID(id);
            post.ViewCount++;
            ps.Update(post);

            return View(Tuple.Create<Post, User,Category ,List<Category>, List<Comment>>(post,us.GetByID(post.UserID),cs.GetByID(post.CategoryID),cs.GetActive(),cms.GetDefault(x => x.Post.ID==id)));
        }

        

        //public IActionResult Privacy()
        //{
        //    return View();
        //}

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public IActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
       
    }
}
