using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class RiderSeason
    {
        public int RiderSeasonId { get; set; }
        public virtual Season Season { get; set; }
        public virtual Rider Rider { get; set; }
        public string Team { get; set; }
    }
}
