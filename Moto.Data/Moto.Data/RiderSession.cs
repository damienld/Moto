using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Moto.Data
{
    public class RiderSession
    {
        public RiderSession()
        {
            ListLapTimes = new List<LapTime>();
        }
        [Browsable(false)]
        public int NbFalls
        {
            get
            {
                return ListLapTimes?.Count(m => m.IsUnFinished) ?? 0;
            }
        }
        [Browsable(false)]
        public bool IsNotClassified { get; set; }
        [Required]
        public virtual Session Session { get; set; }
        
        public int RiderSessionId { get; set; }
        public virtual RiderSeason RiderSeason { get; set; }
        [Description("Rk")]
        [Display(Name = "Rk")]
        public int Rank
        { get; set; }
        [Description("Name")]
        [Display(Name = "Name")]
        public string RiderName
        { get; set; }
        [Description("")]
        [Display(Name = "")]
        public string RiderFirstName
        { get; set; }
        [Browsable(false)]
        [NotMapped]
        public string RiderDisplayName
        {
            get
            {
                try
                {
                    return $"{RiderFirstName.Substring(0, 1)}.{RiderName[0]}{RiderName.ToLower().Substring(1)}";
                }
                catch (Exception)
                {
                    return RiderName;                    
                }
            }
        }
        [Browsable(false)]
        [NotMapped]
        public string RiderDisplayNameShort
        {
            get
            {
                try
                {
                    return $"{RiderDisplayName.Substring(0, Math.Min(5, RiderDisplayName.Length))}";
                }
                catch (Exception)
                {
                    return RiderDisplayName;
                }
            }
        }
        [Description("#")]
        [Display(Name = "#")]
        public string RiderNumber
        { get; set; }
        private int? nbFullLaps;
        [Description("Laps")]
        [Display(Name = "Laps")]
        public int? NbFullLaps
        { get => getListFullLaps().Count;

            set
            {
                nbFullLaps = value;
            }
        }
        [Description("Runs")]
        [Display(Name = "Runs")]
        public int? NbRuns
        { get; set; }
        public int? NbLaps
        { get; set; }
        public virtual ICollection<LapTime> ListLapTimes
        { get; set; }

        public List<LapTime> getListBestXLaps(int aNbLaps, int minTyreLapsFront = 0, int minTyreLapsRear = 0)
        {
            List<LapTime> _CountedLaps = getListFullLaps();
            if (minTyreLapsFront > 0)
                _CountedLaps = _CountedLaps.Where(l => l.NbLapsFrontTyre >= minTyreLapsFront).ToList();
            if (minTyreLapsRear > 0)
                _CountedLaps = _CountedLaps.Where(l => l.NbLapsRearTyre >= minTyreLapsRear).ToList();
            if (_CountedLaps == null || _CountedLaps.Count < aNbLaps)
                return null;
            return _CountedLaps.OrderBy(l =>l.Time).ToList().GetRange(0, aNbLaps);
        }
        public List<LapTime> getListLaps(int minTyreLapsFront, int minTyreLapsRear)
        {
            List<LapTime> _CountedLaps = getListFullLaps();
            if (minTyreLapsFront > 0)
                _CountedLaps = _CountedLaps.Where(l => l.NbLapsFrontTyre >= minTyreLapsFront).ToList();
            if (minTyreLapsRear > 0)
                _CountedLaps = _CountedLaps.Where(l => l.NbLapsRearTyre >= minTyreLapsRear).ToList();
            if (_CountedLaps == null )
                return null;
            return _CountedLaps.OrderBy(l => l.Time).ToList();
        }
        public List<LapTime> getListFullLaps()
        {
            if (ListLapTimes == null)
                return new List<LapTime>();
            return ListLapTimes.Where(m => !m.IsUnFinished && !m.IsCancelled
            && !m.IsPitStop && m.Time != null && m.Time > 0).ToList();
        }
        [Description("Avg best 5 laps")]
        public decimal AvgBest5Laps
        {
            get
            {
                List<LapTime> _TopLaps = getListBestXLaps(5);
                if (_TopLaps == null)
                    return 0;
                else
                    return Math.Round(_TopLaps.Average(m=>m.Time.Value), 1);
            }
        }
        public decimal getAvgBestXLaps(int laps, int minTyreLapsFront=0, int minTyreLapsRear=0)
        {
            List<LapTime> _TopLaps = getListBestXLaps(laps, minTyreLapsFront, minTyreLapsRear);
            if (_TopLaps == null)
                return 0;
            else
                return Math.Round(_TopLaps.Average(m => m.Time.Value), 1);
        }
        public string Lap1
        {
            get
            {
                int _nb = 1;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb-1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap2
        {
            get
            {
                int _nb = 2;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap3
        {
            get
            {
                int _nb = 3;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap4
        {
            get
            {
                int _nb = 4;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap5
        {
            get
            {
                int _nb = 5;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap6
        {
            get
            {
                int _nb = 6;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap7
        {
            get
            {
                int _nb = 7;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        public string Lap8
        {
            get
            {
                int _nb = 8;
                List<LapTime> _l = getListFullLaps().OrderBy(l => l.Time).ToList();
                if (_l != null && _l.Count >= _nb)
                {
                    decimal d = _l[_nb - 1].Time.Value;
                    return Math.Round(d, 1) + "";
                }
                else
                    return "";
            }
        }
        
        public static void simulateTyreUseIfNotAvailable(List<RiderSession> list)
        {
            //List<RiderSession> resultList = new List<RiderSession>();
            if (list == null || list.Count <= 0)
                return;
            foreach (var riderSession in list)
            {
                simulateTyreUseIfNotAvailable(riderSession);
            }
        }
        public static void simulateTyreUseIfNotAvailable(RiderSession riderSession)
        {
            //List<RiderSession> resultList = new List<RiderSession>();
            if (riderSession == null || riderSession.ListLapTimes.Count() <= 0)
                return;
            int lastPit = 0;
            //RiderSession newRiderSession = new RiderSession();
            for (int i = 0; i < riderSession.ListLapTimes.Count(); i++)
            {
                LapTime lapTime = riderSession.ListLapTimes.ElementAt(i);
                if (!lapTime.FrontTyreType.HasValue && !lapTime.RearTyreType.HasValue)
                {
                    lapTime.NbLapsFrontTyre = i + 1 - lastPit;
                    lapTime.NbLapsRearTyre = i + 1 - lastPit;
                }
                if (lapTime.IsPitStop)
                    lastPit = i + 1;
            }
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pointsForRanking">Send an empty array to get the default array {25,20,16,13,11,10,9,8,7,6,5,4,3,2,1 }</param>
        /// <returns></returns>
        public int getWorldStandingPoints(int[] pointsForRanking)
        {
            if (pointsForRanking.Length <= 0)
                pointsForRanking = new[] {25,20,16,13,11,10,9,8,7,6,5,4,3,2,1 };
            if (this.Rank <= pointsForRanking.Length)
            {
                Console.WriteLine(this.RiderName+ ":" + pointsForRanking[this.Rank - 1] + " " + this.Session.SessionType);
                if (IsNotClassified)
                    //means rider ended with unfinished lap
                    return 0;
                else return pointsForRanking[this.Rank - 1];
            }
            else
                return 0;
        }
    }
    
}
