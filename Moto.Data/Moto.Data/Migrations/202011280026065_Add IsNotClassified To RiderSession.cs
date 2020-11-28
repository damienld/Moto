namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsNotClassifiedToRiderSession : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.RiderSessions", "IsNotClassified", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.RiderSessions", "IsNotClassified");
        }
    }
}
