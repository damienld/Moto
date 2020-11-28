using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public enum Categories { c125, c250, c500}
    public class Season: NotificationClass
    {
        public Season()
        {
            GPs = new ObservableCollection<Gp>();
        }
        public int SeasonId { get; set; }
        public int Year { get; set; }
        public Categories Category { get; set; }
        public virtual ICollection<Gp> GPs { get; set; }
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
            return $"{this.Year}-{this.Category}";
        }
    }
}
