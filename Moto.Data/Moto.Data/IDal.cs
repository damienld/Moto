using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Moto.Data
{
    public interface IDal
    {
        MotoDataContext Db { get; set; }

        Gp AddGp(int year, Categories category, int gpIndexInSeason, DateTime date, string name, string urlWeather, string note);
        void AddOrReplaceSession(Gp gp, Session session);
        Rider AddRider(string name, string firstname, string note = "");
        RiderSeason AddRiderSeason(Season season, Rider rider, string team);
        Season AddSeasonsIfDontExist(int year, Categories category);
        List<Season> AddSeasonsIfDontExist(List<(int, Categories)> seasonsArgs);
        User AddUser(User user, string key, bool isValidation = true);
        void Dispose();
        List<Gp> getAllGp();
        List<Gp> getAllGp(Categories category);
        List<Gp> getAllGp(int year, Categories category);
        ObservableCollection<Season> getAllSeasons(int? year, Categories? category);
        Gp getGp(int year, Categories category, int gpIdInSeason);
        List<Session> GetGpSessions(Gp selectedGp);
        List<Session> GetGpSessions(int gpId);
        RiderSession GetRiderSession(int riderSessionId);
        List<RiderSession> GetRiderSessions(int sessionId);
        Season getSeason(int year, Categories category);
        User GetUser(string username, string password, string key);
        void RemoveSession(Session session);
        void SetListRiderSession(Session session, List<RiderSession> riderSessions);
        User UpdateUser(User user, string key, bool isValidation = true);
        List<RiderStats> MakeRiderStats(int seasonId);
    }
}