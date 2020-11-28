using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moto.Data
{

    public class RiderStats
    {
        public string RiderName { get; set; }
        public List<RiderSession> RiderSessions { get; set; }

        [Display(Name = "Qual Rk")]
        public int QualifyingRank { get; set; }
        [Display(Name = "Qual Pts")]
        public int QualifyingPts
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int ptsInQ2 = RiderSessions.Where(r => r.Session.SessionType == SessionType.Q2)
                    ?.Sum(p=>p.getWorldStandingPoints(new int[] { }))??0;
                int ptsInQ1 = RiderSessions.Where(r => r.Session.SessionType == SessionType.Q1)
                    ?.Sum(p => p.getWorldStandingPoints(new int[] {0,0,3,2,1 }))??0;
                return ptsInQ2 + ptsInQ1;
            }
        }
        [Display(Name = "Race Rk")]
        public int RaceRank { get; set; }
        [Display(Name = "Race Pts")]
        public int RacePoints
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                List<RiderSession> l = RiderSessions.Where(r => r.Session.IsTheRightRace()).ToList();
                int pts = l
                    ?.Sum(p => p.getWorldStandingPoints(new int[] { })) ?? 0;
                return pts;
            }
        }
        [Display(Name = "Wup Rk")]
        public int WupRank { get; set; }
        [Display(Name = "Wup Pts")]
        public int WupPoints
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int pts = RiderSessions.Where(r => r.Session.SessionType == SessionType.WUP)
                    ?.Sum(p => p.getWorldStandingPoints(new int[] { })) ?? 0;
                return pts;
            }
        }
        [Display(Name = "FP4 Rk")]
        public int Fp4Rank { get; set; }
        [Display(Name = "FP4 Pts")]
        public int Fp4Points
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int pts = RiderSessions.Where(r => r.Session.SessionType == SessionType.FP4)
                    ?.Sum(p => p.getWorldStandingPoints(new int[] { })) ?? 0;
                return pts;
            }
        }
    }
}
