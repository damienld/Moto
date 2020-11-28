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
        //paramterless constructor NEEDED for scaffolding
        public MotoDataContext() : base("name=cnnMotoDb")
        {
            //Database.SetInitializer<MotoDataContext>(new MotoDbInitializer());
        }
        public MotoDataContext(string nameOrConnectionString)
            //: base(isConnectionString?$"name={nameDB}":nameDB)//("name=MotoGpDBConnectionString")
            : base(nameOrConnectionString)
        {
            //Database.SetInitializer<MotoDataContext>(new MotoDbInitializer());
        }
        /*protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Session>().HasMany(e => e.RiderSessions).WithOptional(s => s.Session).WillCascadeOnDelete(true);
            modelBuilder.Entity<RiderSession>().HasMany(e => e.ListLapTimes).WithOptional(s => s.).WillCascadeOnDelete(true);
            //...

            modelBuilder.Entity<Session>()
                .HasMany<RiderSession>(c => c.SessionId)
                .WithOptional(x => x.Parent)
                .WillCascadeOnDelete(true);
        }*/
        public DbSet<Gp> GPs { get; set; }
        public DbSet<LapTime> LapTimes { get; set; }
        public DbSet<Rider> Riders { get; set; }
        public DbSet<RiderSeason> RiderSeasons { get; set; }
        public DbSet<RiderSession> RiderSessions { get; set; }
        public DbSet<Season> Seasons { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<User> Users { get; set; }

    }
}
