using System;
using System.ComponentModel;

namespace Moto.Data
{
    public enum TyreType {Soft, Medium, Hard, SoftWet, MediumWet, HardWet};
    public class LapTime
    {
        public long LapTimeId { get; set; }
        [Description("#")]
        public int IndexLap
        { get; set; }
        [Description("Time")]
        public decimal? Time { get; set; }
        [Description("T1")]
        public decimal? T1 { get; set; }
        public bool T1isCancelled { get; set; }
        [Description("T2")]
        public decimal? T2 { get; set; }
        public bool T2isCancelled { get; set; }
        [Description("T3")]
        public bool T3isCancelled { get; set; }
        public decimal? T3 { get; set; }
        [Description("T4")]
        public bool T4isCancelled { get; set; }
        public decimal? T4 { get; set; }
        [Description("canc.")]
        public bool IsCancelled
        { get; set; }
        [Description("pit")]
        public bool IsPitStop
        { get; set; }
        [Description("ab.")]
        public bool IsUnFinished
        { get; set; }
        [Description("Front")]
        public TyreType? FrontTyreType { get; set; }
        [Description("Lap")]
        public int? NbLapsFrontTyre { get; set; }
        [Description("Rear")]
        public TyreType? RearTyreType { get; set; }
        [Description("Lap")]
        public int? NbLapsRearTyre { get; set; }

        /// <summary>
        /// Can be under 2 forms(max speed is inserted in N-1 column except for lap 1):
        /// 1 1'43.072 23.107 32.454 15.453 32.058
        /// 2 1'40.091 20.921 31.977 15.328 333.3 31.865
        /// </summary>
        /// <param name="_tabLineLap"></param>
        public void readLapAndSectorsTimeFromAnalysis(string _lineLap, SessionType sessionType)
        {
            string[] _tabLineLap = _lineLap.Replace(" P ", " ").Split(' ');//Remove " P "before reading
            (this.Time, this.IsCancelled) = readLapTimeFromAnalysis(_tabLineLap[1]);
            if (!(sessionType == SessionType.Race || sessionType == SessionType.Race2))
                return;//only collect T1 T2... for races
            Console.Write("T1: ");
            (this.T1, this.T1isCancelled) = readLapTimeFromAnalysis(_tabLineLap[2]);
            Console.Write("T2: ");
            (this.T2, this.T2isCancelled) = readLapTimeFromAnalysis(_tabLineLap[3]);
            Console.Write("T3: ");
            (this.T3, this.T3isCancelled) = readLapTimeFromAnalysis(_tabLineLap[4]);
            Console.Write("T4: ");
            //Max Speed comes before T4 (except for T1 or some other laps)
            if ((_tabLineLap[_tabLineLap.Length - 1].Replace("*","").Split('.'))[1].Length==3) 
            //if 3 digits after "." => it s a sector time
                (this.T4, this.T4isCancelled) = readLapTimeFromAnalysis(_tabLineLap[_tabLineLap.Length-1]);
            else//else it s the max speed, need to go to the previous column to read T4
                (this.T4, this.T4isCancelled) = readLapTimeFromAnalysis(_tabLineLap[_tabLineLap.Length - 2]);
            if (T1+T2+T3+T4 != Time)
            {
                Console.WriteLine("Error reading split times for:" + String.Join(" ", _tabLineLap));
                //throw new Exception("Error reading split times for:" + String.Join(" ", _tabLineLap));
            }
        }

        /// <summary>
        /// read example: 1'42.656
        /// </summary>
        /// <param name="_tabLineLap"></param>
        /// <returns></returns>
        private static (decimal?, bool) readLapTimeFromAnalysis(string _tabLineLap)
        {
            try
            {
                bool isCancelledTime = _tabLineLap.Contains("*");
                string[] _tabTime = _tabLineLap.Replace('*', ' ').Replace("'", ".").Trim().Split('.');
                if (_tabTime.Length == 2 || _tabTime.Length == 3)
                {
                    decimal time = 0;
                    if (_tabTime.Length == 3)
                        time = Decimal.Parse(_tabTime[0]) * 60 + Decimal.Parse(_tabTime[1])
                        + Decimal.Parse(_tabTime[2]) / 1000;
                    else
                        time = Decimal.Parse(_tabTime[0])
                        + Decimal.Parse(_tabTime[1]) / 1000;
                    Console.WriteLine(time.ToString() + (isCancelledTime ? "*" : ""));
                    return (time, isCancelledTime);
                }
                else return (null, false);
            }
            catch (Exception)
            {
                return (null, false);
            }
        }

    }

}
