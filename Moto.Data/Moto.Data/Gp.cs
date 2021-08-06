using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class Gp : NotificationClass
    {
        public int getNbRiderFromQ1ToQ2()
        {
            if (this.Season.Category == Categories.MotoGP)
                return 2;
            else if (this.Season.Category == Categories.Moto2)
                return 4;
            else //if (this.Season.Category == Categories.Moto3)
                return 4;
        }
        public Gp()
        {
            Sessions = new List<Session>();
            Date = DateTime.Today;
            if (Season != null && Season.GPs != null)
                GpIdInSeason = Season.GPs.Count + 1;
        }

        public long GpId { get; set; }
        [Description("Id")]
        [Display(Name="Id")]
        public int GpIdInSeason { get; set; }
        [Description("GP")]
        [Display(Name = "GP")]
        public string Name { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}")]
        public DateTime Date { get; set; }
        public string UrlWeather { get; set; } = "http://www.weather.com";
        public string Note { get; set; }
        [Required]
        public virtual Season Season { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
        
        public string getLabel(int? Length)
        {
            string str = this.ToString();
            if (Length.HasValue)
                return str.Substring(0, Math.Min(Length.Value - 1, str.Length - 1));
            else
                return str;
        }
        public override string ToString()
        {
            return $"{this.GpIdInSeason}-{this.Name}-{this.Date.Year}";// -{this.Season.Category}";
        }

    }
}
