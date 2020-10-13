using System;
using System.Collections.Generic;
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
        }

        public long GpId { get; set; }
        public int GpIdInSeason { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public Season Season { get; set; }
        public virtual ICollection<Session> Sessions { get; set; }

    }
}
