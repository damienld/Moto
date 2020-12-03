using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Moto.Data
{
    public enum SessionType { FP1, FP2, FP3, FP4, Q1, Q2, WUP, Race, Race2}
    public class Session
    {
        public string CreateCsvRace()// List<RiderSession> riderSessions)
        {
            List<RiderSession> riderSessions = this.RiderSessions.ToList();
            int nbLapsRace = riderSessions.First().ListLapTimes.Count;
            string res = $"Rider,Team,ImageURL,";
            for (int i = 1; i <= nbLapsRace - 1; i++)
            {
                res += ",,,,"+ (nbLapsRace - i) + "Lap(s) to Go";
            }
            res += ",,,,FINISH!!!";
            res += Environment.NewLine;
            foreach (var riderSession in riderSessions)
            {
                try
                {
                    string arrayCumulatedLapTimes = "0,";
                    decimal cumulLapTime = 0;
                    for (int indexLap = 1; indexLap <= nbLapsRace; indexLap++)
                    {
                        if (riderSession.ListLapTimes.Count < indexLap)
                        {
                            arrayCumulatedLapTimes += $"{cumulLapTime},{cumulLapTime},{cumulLapTime},{cumulLapTime},";
                        }
                        else
                        {
                            LapTime lapTime = riderSession.ListLapTimes.ElementAt(indexLap-1);
                            if (lapTime.T1.HasValue)
                                arrayCumulatedLapTimes += (cumulLapTime + lapTime.T1) + ",";
                            else
                                arrayCumulatedLapTimes += (cumulLapTime) + ",";
                            if (lapTime.T1.HasValue && lapTime.T2.HasValue)
                                arrayCumulatedLapTimes += (cumulLapTime + lapTime.T1 + lapTime.T2) + ",";
                            else
                                arrayCumulatedLapTimes += (cumulLapTime) + ",";
                            if (lapTime.T1.HasValue && lapTime.T2.HasValue && lapTime.T3.HasValue)
                                arrayCumulatedLapTimes += (cumulLapTime + lapTime.T1 + lapTime.T2 + lapTime.T3) + ",";
                            else
                                arrayCumulatedLapTimes += (cumulLapTime) + ",";
                            if (lapTime.Time.HasValue)
                            {
                                arrayCumulatedLapTimes += (cumulLapTime + lapTime.Time) + ",";
                                cumulLapTime += lapTime.Time.Value;
                            }
                            else arrayCumulatedLapTimes += (cumulLapTime) + ",";
                        }
                    }
                    res += $"{riderSession.RiderDisplayName},,,{arrayCumulatedLapTimes}" + Environment.NewLine;

                }
                catch (Exception ex)
                {

                }
                //System.IO.File.WriteAllText("d:/" + riderSessions.First().Session.Gp.Season.Category +"-" 
                //    + riderSessions.First().Session.Gp.Name + ".csv", res);
            }
            Console.WriteLine(res);
            return res;
        }

        public Session()
        {
            RiderSessions = new ObservableCollection<RiderSession>();
        }
        [Browsable(false)]
        public int NbFalls 
        {
            get
            {
                return RiderSessions?.Sum(m => m.NbFalls)??0;
            }
        }
        [Description("Session")]
        [Display(Name ="Session")]
        public SessionType SessionType { get; set; }
        public long SessionId { get; set; }
        //public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        [Description("Ground(°C)")]
        [Display(Name = "Ground(°C)")]
        public int? GroundTemperature { get; set; }
        [Description("Wet")]
        [Display(Name = "Wet")]
        public bool IsWet { get; set; }
        [Required]
        public virtual Gp Gp { get; set; }
        public virtual ICollection<RiderSession> RiderSessions { get; set; }
        [NotMapped]
        public string Label
        {
            get
            {
                return this.ToString();
            }
        }
        public override string ToString()
        {
            return $"{this.SessionType}-{this.Gp?.Name??""}-{this.Gp?.Season.Category.ToString()??""}";
        }
        public static List<RiderSession> ReadAnalysisPdf(string _data, SessionType sessionType)
        {
            List<string> lines = _data.Replace("\r\n", "£").Split('£').ToList();

            lines = removeUselessLines(lines);

            if (lines.FindIndex(l => l.Contains("Run #")) > -1)
                return ReadLinesRidersAndLapsWithRunsDetails(lines, sessionType);
            else
                return ReadLinesRidersAndLaps(lines, sessionType);
        }
        private static List<string> removeUselessLines(List<string> _data)
        {
            //SortableBindingList<RiderSession> _listRiders = new SortableBindingList<RiderSession>();
            //remove all rows till "Chronological "
            int _index1 = _data.FindIndex(l => l.StartsWith("Chronological "));
            for (int i = 0; i <= _index1; i++)
            {
                _data.RemoveAt(0);
            }
            //remove all interpage between rows "* Page X of X" and "TISSOT"
            Regex _reg = new Regex(@"^.*Page [0-9] of [0-9]$");
            int _index2 = _data.FindIndex(l => _reg.IsMatch(l));
            while (_index2 > -1)
            {
                int _index3 = _data.FindIndex(l => l.Trim().ToUpper() == "TISSOT");
                for (int i = _index2; i <= _index3; i++)
                {
                    _data.RemoveAt(_index2);
                }
                _index2 = _data.FindIndex(l => _reg.IsMatch(l));
            }
            //remove
            int _index4 = _data.FindIndex(l => l.EndsWith(" Speed"));
            while (_index4 > -1)
            {
                _data.RemoveAt(_index4);
                _index4 = _data.FindIndex(l => l.EndsWith(" Speed"));
            }
            int _index5 = _data.FindIndex(l => (l.EndsWith(" Moto2") || l.EndsWith(" Moto3") || l.EndsWith(" MotoGP"))
            && !l.EndsWith("Yamaha MotoGP"));
            while (_index5 > -1)
            {
                _data.RemoveAt(_index5);
                _index5 = _data.FindIndex(l => (l.EndsWith(" Moto2") || l.EndsWith(" Moto3") || l.EndsWith(" MotoGP"))
            && !l.EndsWith("Yamaha MotoGP"));
            }

            return _data;
        }

        public static List<RiderSession> ReadLinesRidersAndLapsWithRunsDetails(List<string> _data, SessionType sessionType)
        {
            List<RiderSession> _listRiders = new List<RiderSession>();
            string strToFindPilots = "Runs=";
            //identify rows with riders names
            int _index = _data.FindIndex(l => l.StartsWith(strToFindPilots)) - 1;
            if (_index < -1)
            {
                strToFindPilots = "Run # 1";
                _index = _data.FindIndex(l => l.StartsWith(strToFindPilots)) - 1;
            }
            int rankRider = 1;
            while (_index > -1)
            {
                try
                {
                    string _lineRider = _data.ElementAt(_index);
                    string[] _tabLine = _lineRider.Split(' ');
                    RiderSession _riderSession = new RiderSession()
                    {
                        RiderName = /*_tabLine[0].Substring(0,1) + "." +*/ _tabLine[1],
                        RiderFirstName = _tabLine[0],
                        Rank = rankRider
                    };
                    rankRider += 1;
                    Console.WriteLine(_tabLine[0] + " " + _tabLine[1]);
                    if (_index >= 0)
                        _riderSession.RiderNumber = _data.ElementAt(_index - 1/*NEW was index*/).Split(' ')[0];
                    string _linelapDetails = _data.ElementAt(_index + 1);
                    if (_linelapDetails.StartsWith("Runs="))
                    {
                        string[] _tabLapDetails = _linelapDetails.Split(' ');
                        string _strFullLaps = _tabLapDetails[_tabLine.Length];
                        string _strNbLaps = _tabLapDetails[_tabLine.Length - 2];
                        string _strNbRuns = _tabLapDetails[_tabLine.Length - 4];
                        _riderSession.NbFullLaps = Convert.ToInt16(_strFullLaps
                            .Substring(_strFullLaps.IndexOf("=") + 1, _strFullLaps.Length - 1 - _strFullLaps.IndexOf("=")));
                        _riderSession.NbLaps = Convert.ToInt16(_strNbLaps
                            .Substring(_strNbLaps.IndexOf("=") + 1, _strNbLaps.Length - 1 - _strNbLaps.IndexOf("=")));
                        _riderSession.NbRuns = Convert.ToInt16(_strNbRuns
                            .Substring(_strNbRuns.IndexOf("=") + 1, _strNbRuns.Length - 1 - _strNbRuns.IndexOf("=")));
                    }
                    _listRiders.Add(_riderSession);
                    for (int i = 0; i < 4; i++)
                    {
                        string line = _data.ElementAt(_index - 1);
                        if (!line.StartsWith("Run"))
                            _data.RemoveAt(_index - 1);
                    }

                    int _indexNextRider = _data.FindIndex(2, l => l.StartsWith(strToFindPilots)) - 2;
                    TyreType? currentFrontTyreType = null;
                    TyreType? currentRearTyreType = null;
                    int? nbLapsFrontTyre = null;
                    int? nbLapsRearTyre = null;
                    int lastIndexLap = 0;
                    //read All Laps/Runs
                    for (int i = _index; i <= (_indexNextRider <= -2 ? _data.Count : _indexNextRider); i++)
                    {
                        string _lineLap = _data.ElementAt(i - 1).Replace(" *","*");
                        Console.WriteLine(_lineLap);
                        string[] _tabLineLap = _lineLap.Split(' ');
                        if (_tabLineLap.Length >= 4 && _tabLineLap[0].StartsWith("Run"))
                        {//Run # 2 Front Tyre Slick-Medium Slick-Medium Rear Tyre
                            try
                            {
                                string rearTyre = _tabLineLap[_tabLineLap.Length - 3];
                                string frontTyre = _tabLineLap[_tabLineLap.Length - 4];
                                TyreType? tyreStrToTyreType(string tyreStr)
                                {
                                    switch (tyreStr.ToLower())
                                    {
                                        case "slick-medium":
                                            return TyreType.Medium;
                                        case "slick-hard":
                                            return TyreType.Hard;
                                        case "slick-soft":
                                            return TyreType.Soft;
                                        case "wet-medium":
                                            return TyreType.MediumWet;
                                        case "wet-hard":
                                            return TyreType.HardWet;
                                        case "wet-soft":
                                            return TyreType.SoftWet;
                                        default:
                                            Console.WriteLine("ERROR: Can't read Tyre");
                                            return null;
                                    }
                                }
                                currentFrontTyreType = tyreStrToTyreType(frontTyre);
                                currentRearTyreType = tyreStrToTyreType(rearTyre);
                                nbLapsFrontTyre = null;
                                nbLapsRearTyre = null;
                            }
                            catch (Exception)
                            {
                            }
                            //New Tyre New Tyre
                            //OR
                            //8 8 Laps at start Laps at start
                            try
                            {
                                string _lineNextLap = _data.ElementAt(i).Replace(" *", "*").Replace("New Tyre", "0");
                                string[] _tabLineNextLap = _lineNextLap.Split(' ');
                                int nbLaps = 0;
                                if (!(int.TryParse(_tabLineNextLap[0], out nbLaps)))
                                    nbLapsFrontTyre = null;
                                else
                                {
                                    Console.WriteLine(String.Format("Front tyre: {0} laps", nbLaps));
                                    nbLapsFrontTyre = nbLaps;
                                }
                                if (!(int.TryParse(_tabLineNextLap[1], out nbLaps)))
                                    nbLapsRearTyre = null;
                                else
                                {
                                    nbLapsRearTyre = nbLaps;
                                    Console.WriteLine(String.Format("Rear tyre: {0} laps", nbLaps));
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }
                        else if (_tabLineLap.Length >= 2 && _tabLineLap[1].Contains("'"))
                        { //if 2nd sequence of the line contains ' (= it's a time)
                            lastIndexLap = Convert.ToInt16(_tabLineLap[0]);
                            if (nbLapsFrontTyre.HasValue)
                                nbLapsFrontTyre += 1;
                            if (nbLapsRearTyre.HasValue)
                                nbLapsRearTyre += 1;
                            LapTime _lapTimes = new LapTime()
                            {
                                IndexLap = lastIndexLap,
                                Time = null,
                                T1 = null,
                                T2 = null,
                                T3 = null,
                                T4 = null,
                                IsCancelled = _tabLineLap[1].Contains("*"),
                                IsPitStop = _lineLap.Contains(" P "),
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished",
                                FrontTyreType = currentFrontTyreType,
                                RearTyreType = currentRearTyreType,
                                NbLapsFrontTyre = nbLapsFrontTyre,
                                NbLapsRearTyre = nbLapsRearTyre
                            };
                            _lapTimes.readLapAndSectorsTimeFromAnalysis(_lineLap, sessionType);
                            _riderSession.ListLapTimes.Add(_lapTimes);
                        }
                        else
                        {//IF it s NOT a time
                            bool isUnFinished = _tabLineLap[0].ToLower() == "unfinished";
                            if (isUnFinished)
                            {
                                _riderSession.ListLapTimes.Add(new LapTime()
                                {
                                    IsUnFinished = isUnFinished,
                                    Time = null,
                                    IndexLap = lastIndexLap
                                });
                                if (nbLapsFrontTyre.HasValue)
                                    nbLapsFrontTyre += 1;
                                if (nbLapsRearTyre.HasValue)
                                    nbLapsRearTyre += 1;
                            }
                            else
                            {
                                Console.WriteLine($"Line not read: {_lineRider}");
                            }
                        }
                    }
                    for (int i = 1; i <= (_indexNextRider == -1 ? _data.Count - 1 : /*Math.Min(*/_indexNextRider/*, _data.Count - 1)*/); i++)
                    {
                        _data.RemoveAt(0);
                    }
                    //_riderSession.ListLapTimes = _riderSession.ListLapTimes/*.Where(m => !m.IsCancelled)*/.OrderBy(m => m.Time).ToList();
                    _riderSession.NbFullLaps = _riderSession.getListFullLaps().Count;
                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message);
                }
                _index = _data.FindIndex(l => l.StartsWith(strToFindPilots)) - 1;
            }
            return _listRiders;
            

        }

        public static List<RiderSession> ReadLinesRidersAndLaps(List<string> _data, SessionType sessionType)
        {
            List<RiderSession> _listRiders = new List<RiderSession>();
            //identify rows with riders names
            bool _isModeWithRuns = true; // if file contains " Runs=" on riders line
            int _index = _data.FindIndex(l => l.Contains(" Runs="));
            if (_index == -1)
            {
                _isModeWithRuns = false;
                _index = _data.FindIndex(l => l.EndsWith("st") || l.EndsWith("nd") || l.EndsWith("rd") || l.EndsWith("th")); //end with 1st 2nd 3rd...
            }
            int _nbLoops = 0;
            int rankRider = 1;
            while (_index > -1 && _nbLoops < 500)
            {
                _nbLoops++;
                try
                {
                    string _lineRider = _data.ElementAt(_index).Replace(" *", "*"); ;
                    string[] _tabLine = _lineRider.Split(' ');
                    RiderSession _rider = new RiderSession()
                    {
                        Rank = rankRider,
                        RiderName = _tabLine[2],
                        RiderFirstName = _tabLine[1],
                        RiderNumber = _tabLine[0]
                            
                    };
                    string _strFullLaps = "";
                    string _strNbLaps = "";
                    string _strNbRuns = "";
                    if (_isModeWithRuns)
                    {
                        _strFullLaps = _tabLine[_tabLine.Length - 1];
                        _strNbLaps = _tabLine[_tabLine.Length - 3];
                        _strNbRuns = _tabLine[_tabLine.Length - 5];
                        _rider.NbFullLaps = Convert.ToInt16(_strFullLaps.Substring(_strFullLaps.IndexOf("=") + 1
                              , _strFullLaps.Length - 1 - _strFullLaps.IndexOf("=")));
                        _rider.NbLaps = Convert.ToInt16(_strNbLaps.Substring(_strNbLaps.IndexOf("=") + 1
                              , _strNbLaps.Length - 1 - _strNbLaps.IndexOf("=")));
                        _rider.NbRuns = Convert.ToInt16(_strNbRuns.Substring(_strNbRuns.IndexOf("=") + 1
                              , _strNbRuns.Length - 1 - _strNbRuns.IndexOf("=")));
                    }
                    rankRider += 1;
                    _listRiders.Add(_rider);
                    _data.RemoveAt(_index);

                    int _indexNextRider = -1;
                    if (_isModeWithRuns)
                        _indexNextRider = _data.FindIndex(l => l.Contains(" Runs="));
                    else
                        _indexNextRider = _data.FindIndex(l => l.EndsWith("st") || l.EndsWith("nd") || l.EndsWith("rd") || l.EndsWith("th")); //end with 1st 2nd 3rd...
                    int lastIndexLap = 0;
                    //All lines for a Rider
                    for (int i = _index; i <= (_indexNextRider == -1 ? _data.Count - 1 : _indexNextRider-1); i++)
                    {
                        string _lineLap = _data.ElementAt(i).Replace(" *", "*"); ;
                        string[] _tabLineLap = _lineLap.Split(' ');
                        if (_tabLineLap.Length >= 1)
                        {
                            lastIndexLap = Convert.ToInt16(_tabLineLap[0]);
                            LapTime _lapTimes = new LapTime()
                            {
                                IndexLap = lastIndexLap,
                                Time = null,
                                T1 = null,
                                T2 = null,
                                T3 = null,
                                T4 = null,
                                IsCancelled = _tabLineLap[1].Contains("*"),
                                IsPitStop = _lineLap.Contains(" P "),
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished"
                            };
                            _lapTimes.readLapAndSectorsTimeFromAnalysis(_lineLap, sessionType);
                            _rider.ListLapTimes.Add(_lapTimes);
                        }
                        else
                        {
                            _rider.ListLapTimes.Add(new LapTime()
                            {
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished",
                                Time = null,
                                IndexLap = lastIndexLap
                            });
                        }
                    }
                    for (int i = _index; i <= (_indexNextRider == -1 ? _data.Count - 1 : Math.Min(_indexNextRider - 1, _data.Count - 1)); i++)
                    {
                        _data.RemoveAt(_index);
                    }
                    //_rider.ListLapTimes = _rider.ListLapTimes.Where(m => !m.IsCancelled).OrderBy(m => m.Time).ToList();
                   // _rider.ListLapTimes = _rider.ListLapTimes.OrderBy(m => m.Time).ToList();
                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message);
                }
                if (_isModeWithRuns)
                    _index = _data.FindIndex(l => l.Contains(" Runs="));
                else
                    _index = _data.FindIndex(l => l.EndsWith("st") || l.EndsWith("nd") || l.EndsWith("rd") || l.EndsWith("th")); //end with 1st 2nd 3rd...
            }
            return _listRiders;


        }

        /// <summary>
        /// Returns true if this session is the final race (for example if it s race 2)
        /// </summary>
        /// <returns></returns>
        public bool IsTheRightRace()
        {
            return (
                (this.SessionType == SessionType.Race 
                && this.Gp.Sessions.FirstOrDefault(s => s.SessionType == SessionType.Race2)==null
                ||
                (this.SessionType == SessionType.Race2)
                ));
        }
    }
}
