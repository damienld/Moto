using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Moto.Data
{
    public class RiderSession
    {
        public RiderSession()
        {
            ListLapTimes = new List<LapTime>();
        }
        [Required]
        public Session Session { get; set; }
        
        public int RiderSessionId { get; set; }
        public virtual RiderSeason RiderSeason { get; set; }
        public int Rank
        { get; set; }
        public string RiderName
        { get; set; }
        public string RiderFirstName
        { get; set; }
        public string RiderNumber
        { get; set; }
        private int? nbFullLaps;
        public int? NbFullLaps
        { get => getListFullLaps().Count;

            set
            {
                nbFullLaps = value;
            }
        }
        public int? NbRuns
        { get; set; }
        public int? NbLaps
        { get; set; }
        public virtual ICollection<LapTime> ListLapTimes
        { get; set; }

        public List<LapTime> getListBestXLaps(int aNbLaps)
        {
            List<LapTime> _CountedLaps = getListFullLaps();
            if (_CountedLaps == null || _CountedLaps.Count < aNbLaps)
                return null;
            return _CountedLaps.OrderBy(l =>l.Time).ToList().GetRange(0, aNbLaps);
        }
        public List<LapTime> getListFullLaps()
        {
            if (ListLapTimes == null)
                return new List<LapTime>();
            return ListLapTimes.Where(m => !m.IsUnFinished && !m.IsCancelled
            && !m.IsPitStop && m.Time != null && m.Time > 0).ToList();
        }
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
        

    }
    
}
