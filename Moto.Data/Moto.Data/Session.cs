using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public enum SessionType { FP1, FP2, FP3, FP4, Q1, Q2, WUP, Race, Race2}
    public class Session
    {
        public Session()
        {
            RiderSessions = new ObservableCollection<RiderSession>();
        }
        public SessionType SessionType { get; set; }
        public long SessionId { get; set; }
        //public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public bool IsWet { get; set; }
        public Gp Gp { get; set; }
        public virtual ICollection<RiderSession> RiderSessions { get; set; }

        public static List<RiderSession> ReadLinesRidersAndLapsWithRunsDetails(List<string> _data)
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
                    string[] _tabLapDetails = _linelapDetails.Split(' ');
                    string _strFullLaps = _tabLapDetails[_tabLine.Length];
                    string _strNbLaps = _tabLapDetails[_tabLine.Length - 2];
                    string _strNbRuns = _tabLapDetails[_tabLine.Length - 4];
                    _riderSession.NbFullLaps = _strFullLaps
                        .Substring(_strFullLaps.IndexOf("=") + 1, _strFullLaps.Length - 1 - _strFullLaps.IndexOf("="));
                    _riderSession.NbLaps = _strNbLaps
                        .Substring(_strNbLaps.IndexOf("=") + 1, _strNbLaps.Length - 1 - _strNbLaps.IndexOf("="));
                    _riderSession.NbRuns = _strNbRuns
                        .Substring(_strNbRuns.IndexOf("=") + 1, _strNbRuns.Length - 1 - _strNbRuns.IndexOf("="));

                    _listRiders.Add(_riderSession);
                    _data.RemoveAt(_index - 1);
                    _data.RemoveAt(_index - 1);
                    _data.RemoveAt(_index - 1);
                    _data.RemoveAt(_index - 1);
                    /*if (strToFindPilots == "Runs=")
                    {
                        _data.RemoveAt(_index - 1);
                        _data.RemoveAt(_index - 1);
                    }*/

                    //read All Laps/Runs
                    int _indexNextRider = _data.FindIndex(l => l.StartsWith(strToFindPilots)) - 2;
                    TyreType? currentFrontTyreType = null;
                    TyreType? currentRearTyreType = null;
                    int? nbLapsFrontTyre = null;
                    int? nbLapsRearTyre = null;
                    for (int i = _index; i <= (_indexNextRider == -2 ? _data.Count - 1 : _indexNextRider); i++)
                    {
                        string _lineLap = _data.ElementAt(i - 1);
                        Console.WriteLine(_lineLap);
                        string[] _tabLineLap = _lineLap.Split(' ');
                        //if 2nd sequence of the line contains ' (= it's a time)
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
                                            return TyreType.SlickMedium;
                                        case "slick-hard":
                                            return TyreType.SlickHard;
                                        case "slick-soft":
                                            return TyreType.SlickSoft;
                                        case "wet-medium":
                                            return TyreType.WetMedium;
                                        case "wet-hard":
                                            return TyreType.WetHard;
                                        case "wet-soft":
                                            return TyreType.WetSoft;
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
                                string _lineNextLap = _data.ElementAt(i).Replace("New Tyre", "0");
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
                        else if (_tabLineLap.Length >= 1 && _tabLineLap[1].Contains("'"))
                        {
                            if (nbLapsFrontTyre.HasValue)
                                nbLapsFrontTyre += 1;
                            if (nbLapsRearTyre.HasValue)
                                nbLapsRearTyre += 1;
                            LapTime _lapTimes = new LapTime()
                            {
                                IndexLap = _tabLineLap[0],
                                Time = null,
                                IsCancelled = _tabLineLap[1].Contains("*") || _lineLap.Contains(" P ")
                                || _lineLap.Contains(" * "),
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished",
                                FrontTyreType = currentFrontTyreType,
                                RearTyreType = currentRearTyreType,
                                NbLapsFrontTyre = nbLapsFrontTyre,
                                NbLapsRearTyre = nbLapsRearTyre
                            };
                            if (!_lapTimes.IsUnFinished && !_lapTimes.IsCancelled)
                            {
                                string[] _tabTime = _tabLineLap[1].Replace('*', ' ').Replace("'", ".").Trim().Split('.');
                                if (_tabTime.Length == 3)
                                {
                                    _lapTimes.Time = Decimal.Parse(_tabTime[0]) * 60 + Decimal.Parse(_tabTime[1])
                                        + Decimal.Parse(_tabTime[2]) / 1000;

                                }
                            }
                            _riderSession.ListLapTimes.Add(_lapTimes);
                        }
                        else
                        {
                            if (nbLapsFrontTyre.HasValue)
                                nbLapsFrontTyre += 1;
                            if (nbLapsRearTyre.HasValue)
                                nbLapsRearTyre += 1;
                            _riderSession.ListLapTimes.Add(new LapTime()
                            {
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished",
                                Time = null
                            });
                        }
                    }
                    for (int i = 1; i <= (_indexNextRider == -1 ? _data.Count - 1 : Math.Min(_indexNextRider, _data.Count - 1)); i++)
                    {
                        _data.RemoveAt(0);
                    }
                    _riderSession.ListLapTimes = _riderSession.ListLapTimes.Where(m => !m.IsCancelled).OrderBy(m => m.Time).ToList();
                    _riderSession.NbFullLaps = _riderSession.getListCountedLaps().Count + "";
                }
                catch (Exception exc)
                {
                    //MessageBox.Show(exc.Message);
                }
                _index = _data.FindIndex(l => l.StartsWith(strToFindPilots)) - 1;
            }
            return _listRiders;
            

        }
        public static List<RiderSession> ReadLinesRidersAndLaps(List<string> _data)
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
                    string _lineRider = _data.ElementAt(_index);
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
                        _rider.NbFullLaps = _strFullLaps.Substring(_strFullLaps.IndexOf("=") + 1
                              , _strFullLaps.Length - 1 - _strFullLaps.IndexOf("="));
                        _rider.NbLaps = _strNbLaps.Substring(_strNbLaps.IndexOf("=") + 1
                              , _strNbLaps.Length - 1 - _strNbLaps.IndexOf("="));
                        _rider.NbRuns = _strNbRuns.Substring(_strNbRuns.IndexOf("=") + 1
                              , _strNbRuns.Length - 1 - _strNbRuns.IndexOf("="));
                    }
                    rankRider += 1;
                    _listRiders.Add(_rider);
                    _data.RemoveAt(_index);

                    int _indexNextRider = -1;
                    if (_isModeWithRuns)
                        _indexNextRider = _data.FindIndex(l => l.Contains(" Runs="));
                    else
                        _indexNextRider = _data.FindIndex(l => l.EndsWith("st") || l.EndsWith("nd") || l.EndsWith("rd") || l.EndsWith("th")); //end with 1st 2nd 3rd...
                    for (int i = _index; i <= (_indexNextRider == -1 ? _data.Count - 1 : _indexNextRider); i++)
                    {
                        string _lineLap = _data.ElementAt(i);
                        string[] _tabLineLap = _lineLap.Split(' ');
                        if (_tabLineLap.Length >= 1)
                        {
                            LapTime _lapTimes = new LapTime()
                            {
                                IndexLap = _tabLineLap[0],
                                Time = null,
                                IsCancelled = _tabLineLap[1].Contains("*") || _lineLap.Contains(" P ") || _lineLap.Contains(" * "),
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished"
                            };
                            if (!_lapTimes.IsUnFinished && !_lapTimes.IsCancelled)
                            {
                                string[] _tabTime = _tabLineLap[1].Replace('*', ' ').Replace("'", ".").Trim().Split('.');
                                if (_tabTime.Length == 3)
                                {
                                    _lapTimes.Time = Decimal.Parse(_tabTime[0]) * 60 + Decimal.Parse(_tabTime[1])
                                        + Decimal.Parse(_tabTime[2]) / 1000;

                                }
                            }
                            _rider.ListLapTimes.Add(_lapTimes);
                        }
                        else
                        {
                            _rider.ListLapTimes.Add(new LapTime()
                            {
                                IsUnFinished = _tabLineLap[0].ToLower() == "unfinished",
                                Time = null
                            });
                        }
                    }
                    for (int i = _index; i <= (_indexNextRider == -1 ? _data.Count - 1 : Math.Min(_indexNextRider - 1, _data.Count - 1)); i++)
                    {
                        _data.RemoveAt(_index);
                    }
                    _rider.ListLapTimes = _rider.ListLapTimes.Where(m => !m.IsCancelled).OrderBy(m => m.Time).ToList();
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

    }
}
