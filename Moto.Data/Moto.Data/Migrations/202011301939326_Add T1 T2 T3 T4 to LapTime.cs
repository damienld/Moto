namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddT1T2T3T4toLapTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.LapTimes", "T1", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LapTimes", "T2", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LapTimes", "T3", c => c.Decimal(precision: 18, scale: 2));
            AddColumn("dbo.LapTimes", "T4", c => c.Decimal(precision: 18, scale: 2));
        }
        
        public override void Down()
        {
            DropColumn("dbo.LapTimes", "T4");
            DropColumn("dbo.LapTimes", "T3");
            DropColumn("dbo.LapTimes", "T2");
            DropColumn("dbo.LapTimes", "T1");
        }
    }
}
