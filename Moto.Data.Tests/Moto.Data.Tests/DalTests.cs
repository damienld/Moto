using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Moto.Data.Tests
{
    [TestClass]
    public class DalTests
    {
        private Dal dal = new Dal("Moto"); //TODO IDal
        [TestInitialize]
        public void Init_AvantChaqueTest()
        {
            IDatabaseInitializer<MotoDataContext> init
                //= new MotoDbInitializer();
                = new DropCreateDatabaseAlways<MotoDataContext>();
            Database.SetInitializer(init);
            init.InitializeDatabase(new MotoDataContext("Moto_Test"));
            InitSeasons2020();
        }

        [TestCleanup]
        public void ApresChaqueTest()
        {
            dal.Dispose();
        }

        private void InitSeasons2020()
        {
            (int year, Categories category) season1
                = (2020, Categories.c125);
            (int year, Categories category) season2
                = (2020, Categories.c250);
            (int year, Categories category) season3
                = (2020, Categories.c500);
            dal.AddSeasonsIfDontExist(new List<(int, Categories)>() { season1, season2, season3 });
            
        }
        [TestMethod]
        public void Check_AddSeasons()
        {
            Season season = dal.getSeason(2020, Categories.c500);
            Assert.IsTrue(season != null);
            season = dal.getSeason(2020, Categories.c250);
            Assert.IsTrue(season != null);
            season = dal.getSeason(2020, Categories.c125);
            Assert.IsTrue(season != null);
        }
        [TestMethod]
        public void Check_AddGp()
        {
            dal.AddGp(2020, Categories.c125, 1, new DateTime(2020, 3, 6), "Qatar", "", "");
            dal.AddGp(2020, Categories.c250, 1, new DateTime(2020, 3, 6), "Qatar", "", "");
            Gp gp = dal.getGp(2020, Categories.c250, 1);
            Assert.IsTrue(gp != null);
            gp = dal.getGp(2020, Categories.c125, 1);
            Assert.IsTrue(gp != null);
        }
        [TestMethod]
        public void Check_AddRider_RiderSeason()
        {
            Season season = dal.getSeason(2020, Categories.c500);
            Rider rider = dal.AddRider("testName", "testFirstName", "[test]");
            RiderSeason riderSeason = dal.AddRiderSeason(season, rider, "teamTest");
            Assert.IsTrue(riderSeason.Rider.Firstname == "testFirstName");
        }
    }
}
