using Moto.Data;
using PMotoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMotoWeb.Controllers
{
    public class LastGpsController : Controller
    {
        private IDal dal;
        public LastGpsController() : this(new Dal("name = cnnMotoDb"))
        {
        }
        public LastGpsController(IDal dalIoc)
        {
            dal = dalIoc;
        }
        // GET: LastGps
        public ActionResult Index()
        {
            HomeViewModel vm = new HomeViewModel();
            // vm.SelectedCategory = Moto.Data.Categories.MotoGP;
            vm.ListGps = dal.getAllGp();// (vm.SelectedCategory);
            return View(vm);
        }
    }
}