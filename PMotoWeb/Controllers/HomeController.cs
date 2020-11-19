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
        private Dal dal;
        public HomeController() : this(new Dal("cnnMotoDb"))
        {
        }
        public HomeController(Dal dalIoc)
        {
            dal = dalIoc;
        }

        public ActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
           // vm.SelectedCategory = Moto.Data.Categories.c500;
            vm.ListGpsForCategory = dal.getAllGp();// (vm.SelectedCategory);
            return View(vm);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}