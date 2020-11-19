namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Gps",
                c => new
                    {
                        GpId = c.Long(nullable: false, identity: true),
                        GpIdInSeason = c.Int(nullable: false),
                        Name = c.String(),
                        Date = c.DateTime(nullable: false),
                        UrlWeather = c.String(),
                        Note = c.String(),
                        Season_SeasonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.GpId)
                .ForeignKey("dbo.Seasons", t => t.Season_SeasonId, cascadeDelete: true)
                .Index(t => t.Season_SeasonId);
            
            CreateTable(
                "dbo.Seasons",
                c => new
                    {
                        SeasonId = c.Int(nullable: false, identity: true),
                        Year = c.Int(nullable: false),
                        Category = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SeasonId);
            
            CreateTable(
                "dbo.Sessions",
                c => new
                    {
                        SessionId = c.Long(nullable: false, identity: true),
                        SessionType = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        Note = c.String(),
                        GroundTemperature = c.Int(),
                        IsWet = c.Boolean(nullable: false),
                        Gp_GpId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.SessionId)
                .ForeignKey("dbo.Gps", t => t.Gp_GpId, cascadeDelete: true)
                .Index(t => t.Gp_GpId);
            
            CreateTable(
                "dbo.RiderSessions",
                c => new
                    {
                        RiderSessionId = c.Int(nullable: false, identity: true),
                        Rank = c.Int(nullable: false),
                        RiderName = c.String(),
                        RiderFirstName = c.String(),
                        RiderNumber = c.String(),
                        NbFullLaps = c.Int(),
                        NbRuns = c.Int(),
                        NbLaps = c.Int(),
                        RiderSeason_RiderSeasonId = c.Int(),
                        Session_SessionId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.RiderSessionId)
                .ForeignKey("dbo.RiderSeasons", t => t.RiderSeason_RiderSeasonId)
                .ForeignKey("dbo.Sessions", t => t.Session_SessionId, cascadeDelete: true)
                .Index(t => t.RiderSeason_RiderSeasonId)
                .Index(t => t.Session_SessionId);
            
            CreateTable(
                "dbo.LapTimes",
                c => new
                    {
                        LapTimeId = c.Long(nullable: false, identity: true),
                        IndexLap = c.Int(nullable: false),
                        Time = c.Decimal(precision: 18, scale: 2),
                        IsCancelled = c.Boolean(nullable: false),
                        IsPitStop = c.Boolean(nullable: false),
                        IsUnFinished = c.Boolean(nullable: false),
                        FrontTyreType = c.Int(),
                        NbLapsFrontTyre = c.Int(),
                        RearTyreType = c.Int(),
                        NbLapsRearTyre = c.Int(),
                        RiderSession_RiderSessionId = c.Int(),
                    })
                .PrimaryKey(t => t.LapTimeId)
                .ForeignKey("dbo.RiderSessions", t => t.RiderSession_RiderSessionId)
                .Index(t => t.RiderSession_RiderSessionId);
            
            CreateTable(
                "dbo.RiderSeasons",
                c => new
                    {
                        RiderSeasonId = c.Int(nullable: false, identity: true),
                        Team = c.String(),
                        Rider_RiderId = c.Int(nullable: false),
                        Season_SeasonId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.RiderSeasonId)
                .ForeignKey("dbo.Riders", t => t.Rider_RiderId, cascadeDelete: true)
                .ForeignKey("dbo.Seasons", t => t.Season_SeasonId, cascadeDelete: true)
                .Index(t => t.Rider_RiderId)
                .Index(t => t.Season_SeasonId);
            
            CreateTable(
                "dbo.Riders",
                c => new
                    {
                        RiderId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Firstname = c.String(),
                        Note = c.String(),
                    })
                .PrimaryKey(t => t.RiderId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RiderSessions", "Session_SessionId", "dbo.Sessions");
            DropForeignKey("dbo.RiderSessions", "RiderSeason_RiderSeasonId", "dbo.RiderSeasons");
            DropForeignKey("dbo.RiderSeasons", "Season_SeasonId", "dbo.Seasons");
            DropForeignKey("dbo.RiderSeasons", "Rider_RiderId", "dbo.Riders");
            DropForeignKey("dbo.LapTimes", "RiderSession_RiderSessionId", "dbo.RiderSessions");
            DropForeignKey("dbo.Sessions", "Gp_GpId", "dbo.Gps");
            DropForeignKey("dbo.Gps", "Season_SeasonId", "dbo.Seasons");
            DropIndex("dbo.RiderSeasons", new[] { "Season_SeasonId" });
            DropIndex("dbo.RiderSeasons", new[] { "Rider_RiderId" });
            DropIndex("dbo.LapTimes", new[] { "RiderSession_RiderSessionId" });
            DropIndex("dbo.RiderSessions", new[] { "Session_SessionId" });
            DropIndex("dbo.RiderSessions", new[] { "RiderSeason_RiderSeasonId" });
            DropIndex("dbo.Sessions", new[] { "Gp_GpId" });
            DropIndex("dbo.Gps", new[] { "Season_SeasonId" });
            DropTable("dbo.Riders");
            DropTable("dbo.RiderSeasons");
            DropTable("dbo.LapTimes");
            DropTable("dbo.RiderSessions");
            DropTable("dbo.Sessions");
            DropTable("dbo.Seasons");
            DropTable("dbo.Gps");
        }
    }
}
