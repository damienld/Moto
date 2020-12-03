using Moto.Data;
using PMotoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMotoWeb.Controllers
{
    public class HomeController : Controller
    {
        private IDal dal;
        public HomeController() : this(new Dal("name = cnnMotoDb"))
        {
        }
        public HomeController(IDal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
           // vm.SelectedCategory = Moto.Data.Categories.MotoGP;
            vm.ListGps = dal.getAllGp().Take(3).ToList();// (vm.SelectedCategory);
            return View(vm);
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Contact()
        {
            return View();
        }
        public ActionResult Replay()
        {
            return View();
        }
        public ActionResult Tennis()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Golf()
        {
            //ViewBag.Message = "Your application description page.";

            return View();
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                dal.Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}