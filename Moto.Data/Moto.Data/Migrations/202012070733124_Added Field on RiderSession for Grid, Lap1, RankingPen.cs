namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedFieldonRiderSessionforGridLap1RankingPen : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiderSessions", "RankOnGrid", c => c.Int());
            AddColumn("dbo.RiderSessions", "PensToAddToRank", c => c.Int());
            AddColumn("dbo.RiderSessions", "RankAfter1Lap", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiderSessions", "RankAfter1Lap");
            DropColumn("dbo.RiderSessions", "PensToAddToRank");
            DropColumn("dbo.RiderSessions", "RankOnGrid");
        }
    }
}
