using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moto.Data
{
    public class RiderStatsForStart
    {
        public RiderStatsForStart()
        {
        }
        public string RiderName { get; set; }
        public List<RiderSession> RiderSessions { get; set; }
        [Display(Name= "Grid")]
        public int AvgGridRank
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return (int)Math.Round(RiderSessions.Average(r => r.RankOnGrid).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Lap1")]
        public int AvgLap1Rank
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return (int)Math.Round(RiderSessions.Average(r => r.RankAfter1Lap).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Lap2")]
        public int AvgLap2Rank
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int nbLaps = 2;
                return (int)Math.Round(RiderSessions
                    .Where(r=>r.RankAfterXLaps.ContainsKey(nbLaps))
                    .Average(r => r.RankAfterXLaps[nbLaps]).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Lap5")]
        public int AvgLap5Rank
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int nbLaps = 5;
                return (int)Math.Round(RiderSessions
                    .Where(r => r.RankAfterXLaps.ContainsKey(nbLaps))
                    .Average(r => r.RankAfterXLaps[nbLaps]).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Lap10")]
        public int AvgLap10Rank
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                int nbLaps = 10;
                return (int)Math.Round(RiderSessions
                    .Where(r => r.RankAfterXLaps.ContainsKey(nbLaps))
                    .Average(r => r.RankAfterXLaps[nbLaps]).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Diff")]
        public int DiffLap1vsGrid
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return (int)Math.Round(RiderSessions.Average(r => r.RankOnGrid - r.RankAfter1Lap).GetValueOrDefault(0F));
            }
        }
        [Display(Name = "Won")]
        public int NbTimes2RkWonAfterLap1
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return RiderSessions.Count(r => r.RankAfter1Lap < r.RankOnGrid -1);
            }
        }
        [Display(Name = "Lost")]
        public int NbTimes2RkLostAfterLap1
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return RiderSessions.Count(r => r.RankAfter1Lap > r.RankOnGrid + 1);
            }
        }
        [Display(Name = "Diff")]
        public int Diff2RkWonLostAfterLap1
        {
            get
            {
                if (RiderSessions == null)
                    return 0;
                return NbTimes2RkWonAfterLap1-NbTimes2RkLostAfterLap1;
            }
        }
    }
}