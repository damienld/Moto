using Moto.Data;
using PMotoWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PMotoWeb.Controllers
{
    public class MotoStatsController : Controller
    {
        private IDal dal;
        public MotoStatsController() : this(new Dal("name = cnnMotoDb"))
        {
        }
        public MotoStatsController(IDal dalIoc)
        {
            dal = dalIoc;
        }// GET: MotoStats
        public ActionResult Index(int? selectedSeasonId)
        {
            MotoStatsViewModel m = new MotoStatsViewModel
            {
                Seasons = dal.getAllSeasons(null, null).ToList()
            };
            if (m.Seasons == null || m.Seasons.Count <= 0)
                return View();
            if (selectedSeasonId.HasValue)
                m.SelectedSeasonId = selectedSeasonId.Value;
            else
                m.SelectedSeasonId = m.Seasons.Where(s=>s.Category==Categories.MotoGP).First().SeasonId;
            m.RiderStats = dal.MakeRiderStats(m.SelectedSeasonId );
            return View(m);
        }
    }
}