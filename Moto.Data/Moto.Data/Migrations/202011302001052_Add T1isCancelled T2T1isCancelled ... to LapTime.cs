namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddT1isCancelledT2T1isCancelledtoLapTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LapTimes", "T1isCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.LapTimes", "T2isCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.LapTimes", "T3isCancelled", c => c.Boolean(nullable: false));
            AddColumn("dbo.LapTimes", "T4isCancelled", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LapTimes", "T4isCancelled");
            DropColumn("dbo.LapTimes", "T3isCancelled");
            DropColumn("dbo.LapTimes", "T2isCancelled");
            DropColumn("dbo.LapTimes", "T1isCancelled");
        }
    }
}
