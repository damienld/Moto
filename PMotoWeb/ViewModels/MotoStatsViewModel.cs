using Moto.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PMotoWeb.ViewModels
{
    public class MotoStatsViewModel
    {
        public List<RiderStats> RiderStats{ get; set; }
        public List<RiderStatsForStart> RiderStatsForStart { get; set; }
        public int SelectedSeasonId { get; set; }

        public List<Season> Seasons { get; set; }
    }
}