namespace Moto.Data
{
    public enum TyreType {SlickSoft, SlickMedium, SlickHard, WetSoft, WetMedium, WetHard};
    public class LapTime
    {
        public long LapTimeId { get; set; }
        public string IndexLap
        { get; set; }
        public decimal? Time
        { get; set; }
        public bool IsCancelled
        { get; set; }
        public bool IsUnFinished
        { get; set; }
        public TyreType? FrontTyreType { get; set; }
        public TyreType? RearTyreType { get; set; }
        public int? NbLapsFrontTyre { get; set; }
        public int? NbLapsRearTyre { get; set; }
    }
    
}
