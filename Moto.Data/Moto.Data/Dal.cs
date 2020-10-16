using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Moto.Data
{
    public class Dal
    {
        public void Dispose()
        {
            Db.Dispose();
        }
        public MotoDataContext Db { get; set; }
        public Dal(string nameDB) => Db = new MotoDataContext(nameDB);
        public ObservableCollection<Season> getAllSeasons()
        {
            return new ObservableCollection<Season>(Db.Seasons);
        }
        public Season getSeason(int year, Categories category)
        {
            Season season = Db.Seasons.FirstOrDefault(s => year == s.Year && category == s.Category);
            return season;
        }
        /// <summary>
        /// Add the season if it exists into the DB, else return the existing season.
        /// </summary>
        /// <param name="year"></param>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Season> AddSeasonsIfDontExist(List<ValueTuple<int, Categories>> seasonsArgs)//(int year, Categories category)
        {
            List < Season > listNewSeasons = new List<Season>();
            bool hasSeasonsChanged = false;
            foreach (var seasonArgs in seasonsArgs)
            {
                int year = seasonArgs.Item1;
                Categories category = seasonArgs.Item2;
                Season season = getSeason(year, category);
                if (season == null)
                {
                    Season newSeason = Db.Seasons.Add(new Season()
                    {
                        Year = year
                        ,
                        Category = category
                    });
                    hasSeasonsChanged = true;
                    listNewSeasons.Add(newSeason);
                } 
                else listNewSeasons.Add(season);
            }
            if (hasSeasonsChanged)
                Db.SaveChanges();
            return listNewSeasons;
        }
        
        /// <summary>
        /// Add the season if it exists into the DB, else return the existing season.
        /// </summary>
        /// <param name="season"></param>
        /// <returns></returns>
        public Season AddSeasonsIfDontExist(int year, Categories category)
        {
            Season season = getSeason(year, category);
            if (season == null)
            {
                Season newSeason = Db.Seasons.Add(new Season()
                {
                    Year = year
                    ,
                    Category = category
                });
                Db.SaveChanges();
            }
            return season;
        }

        public Gp getGp(int year, Categories category, int gpIdInSeason)
        {
            Gp gp = Db.GPs.FirstOrDefault(g => year == g.Season.Year && category == g.Season.Category
            && g.GpIdInSeason == gpIdInSeason);
            return gp;
        }
        public List<Gp> getAllGp(int year, Categories category)
        {
            List<Gp> gps = Db.GPs.Where(g => year == g.Season.Year && category == g.Season.Category).ToList();
            return gps;
        }
        public List<Gp> getAllGp(Categories category)
        {
            List<Gp> gps = Db.GPs.Where(g => category == g.Season.Category).OrderByDescending(g => g.Date).ToList();
            return gps;
        }
        /// <summary>
        /// Add the Gp (not checking if already exist).
        /// </summary>
        /// <param name="year"></param>
        /// <param name="category"></param>
        /// <returns>NullReferenceException if season don't exist</returns>
        public Gp AddGp(int year, Categories category, int gpIndexInSeason, DateTime date, string name
            , string urlWeather, string note)
        {
            Season season = getSeason(year, category);
            if (season == null)
                throw new NullReferenceException(String.Format("No existing Season for {0} {1}", year, category));
            Gp gp = new Gp()
            {
                Date = date,
                GpIdInSeason = gpIndexInSeason,
                Name = name,
                Season = season,
                Note = note,
                UrlWeather = urlWeather
            };
            season.GPs.Add(gp);
            Db.SaveChanges();
            return gp;
        }

        public void AddOrReplaceSession(Gp gp, Session session)
        {
            Session existingSimilarSession = gp.Sessions.FirstOrDefault(s => s.SessionType == session.SessionType);
            if (existingSimilarSession == null)
                gp.Sessions.Add(session);
            else
                existingSimilarSession = session;
            Db.SaveChanges();
        }

        public void SetListRiderSession(Session session, List<RiderSession> riderSessions)
        {
            session.RiderSessions = riderSessions;
            Db.SaveChanges();
        }

        public Rider AddRider(string name, string firstname, string note="")
        {
            Rider riderCreated = Db.Riders.Add(new Rider() { Firstname = firstname, Name = name, Note = note });
            Db.SaveChanges();
            return riderCreated;
        }
        public RiderSeason AddRiderSeason (Season season, Rider rider, string team)
        {
            RiderSeason riderSeason = 
                Db.RiderSeasons.Add(new RiderSeason() { Season = season, Rider = rider, Team = team });
            Db.SaveChanges();
            return riderSeason;
        }
    }
}
