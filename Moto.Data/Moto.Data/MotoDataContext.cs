using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class MotoDataContext: DbContext
    {
        public MotoDataContext(): base("MotoGp")//("name=MotoGpDBConnectionString")
        {
            Database.SetInitializer<MotoDataContext>(new MotoDbInitializer());
        }

        public DbSet<Gp> GPs { get; set; }
        public DbSet<LapTime> LapTimes { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<RiderSeason> RiderSeasons { get; set; }
        public DbSet<RiderSession> RiderSessions { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Session> Sessions { get; set; }

    }
}
