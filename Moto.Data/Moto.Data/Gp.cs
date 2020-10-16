using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class Gp : NotificationClass
    {
        public Gp()
        {
            Sessions = new List<Session>();
            Date = DateTime.Today;
            if (Season != null && Season.GPs != null)
                GpIdInSeason = Season.GPs.Count + 1;
        }

        public long GpId { get; set; }
        public int GpIdInSeason { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        [Required]
        public Season Season { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }
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
            return $"{this.GpIdInSeason}-{this.Name}-{this.Date}-{this.Season.Category}";
        }

    }
}
