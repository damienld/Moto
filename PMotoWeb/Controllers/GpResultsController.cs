using Moto.Data;
using PMotoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace PMotoWeb.Controllers
{
    public class GpResultsController : Controller
    {
        private IDal dal;
        public GpResultsController() : this(new Dal("name=cnnMotoDb"))
        {
        }
        public GpResultsController(IDal dalIoc)
        {
            dal = dalIoc;
        }
        // GET: GpResults
        public ActionResult Index(int? id)
        {
            if (!id.HasValue)
                return View();
            this.ViewBag.Title = "GP";
            this.ViewBag.IdGp = "id";
            GpResultsViewModel vm = new GpResultsViewModel
            {
                Sessions = dal.GetGpSessions(id.Value)
            };
            if (vm.Sessions != null & vm.Sessions.Count() > 0)
            {
                vm.SelectedSession = vm.Sessions?.Last() ?? null;
                vm.RiderSessions = vm.SelectedSession.RiderSessions.ToList();
            }
            else vm.RiderSessions = new List<RiderSession>();
            return View(vm);
        }
        public ActionResult ListRiderSession(int? idSession)
        {
            if (!idSession.HasValue)
                return PartialView(new List<RiderSession>());
            GpResultsViewModel vm = new GpResultsViewModel()
            {
                RiderSessions = dal.GetRiderSessions(idSession.Value),
            };
            //if (vm.RiderSessions != null && vm.RiderSessions.Count > 0 
            //    && vm.RiderSessions.First().Session.Gp.Season.Category != Categories.MotoGP)
            //    RiderSession.simulateTyreUseIfNotAvailable(vm.RiderSessions);

            if (vm.RiderSessions != null & vm.RiderSessions.Count() > 0)
            {
                vm.SelectedRiderSession = vm.RiderSessions?.First() ?? null;
                vm.SelectedSession = vm.SelectedRiderSession.Session;
            }
            return PartialView(vm);
        }
        public class RiderSessionChart
        {
            public string RiderName
            { get; set; }
            public decimal Lap1
            { get; set; }
            public decimal Avg
            { get; set; }
            public decimal AvgUsedTyres
            { get; set; }
            public int NbLapsForAvgUsedTyres
            { get; set; }
        }
        public JsonResult RiderSessionsToChart(int? idSession,int? nbLapsForAvg, int? nbLapsForAvgWithTyres
            , int? minTyreLapsFront, int? minTyreLapsRear, int? riderRange)
        {
            if (!idSession.HasValue || !nbLapsForAvg.HasValue
                 || !minTyreLapsFront.HasValue || !minTyreLapsRear.HasValue || !riderRange.HasValue)
                return new JsonResult();
            List<RiderSession> list = dal.GetRiderSessions(idSession.Value);
            if (list!=null && list.Count > 0 && list.First().Session.Gp.Season.Category != Categories.MotoGP)
                RiderSession.simulateTyreUseIfNotAvailable(list);

            if (riderRange.Value == 1)
                //first 8 riders
                list = list.Take(8).ToList();
            else if (riderRange.Value == 2)
            {//1st + 8-15th riders
                list = list.Where(r => (r.Rank==1)||(r.Rank >= 9 && r.Rank <= 15)).ToList();
            }
            else if (riderRange.Value == 3)
            {//1st + 8-15th riders
                list = list.Where(r => (r.Rank == 1) || (r.Rank >= 16 && r.Rank <= 23)).ToList();
            }

            decimal res = 0;
            List<RiderSessionChart> listChart 
                = list.Select(p => new RiderSessionChart { RiderName = p.RiderFirstName.Substring(0,1)+"."+ p.RiderName
                , Lap1 = Decimal.TryParse(p.Lap1, out res)?res:0
                , Avg = p.getAvgBestXLaps(nbLapsForAvg.Value)
                , AvgUsedTyres = p.getAvgBestXLaps(nbLapsForAvgWithTyres.Value, minTyreLapsFront.Value, minTyreLapsRear.Value)
                , NbLapsForAvgUsedTyres = p.getListLaps(minTyreLapsFront.Value, minTyreLapsRear.Value)?.Count()??0
                }).ToList();

            return Json(new { JSONList = listChart }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult RiderSessionDetails(int? idRiderSession)
        {
            if (!idRiderSession.HasValue)
                return PartialView(new RiderSession());
            RiderSession session = dal.GetRiderSession(idRiderSession.Value);
            if (session.Session.Gp.Season.Category != Categories.MotoGP)
                RiderSession.simulateTyreUseIfNotAvailable(session);
            return PartialView(session);
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
