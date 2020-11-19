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
        public decimal? Time
        { get; set; }
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
    }
    
}
