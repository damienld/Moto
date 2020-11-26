namespace Moto.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SimplifiedUserClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "Username", c => c.String(nullable: false, maxLength: 30));
            CreateIndex("dbo.Users", "Username", unique: true);
            DropColumn("dbo.Users", "Name");
            DropColumn("dbo.Users", "Phone");
            DropColumn("dbo.Users", "Age");
            DropColumn("dbo.Users", "City");
            DropColumn("dbo.Users", "Address");
            DropColumn("dbo.Users", "DateOfBirth");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Users", "DateOfBirth", c => c.DateTime(nullable: false));
            AddColumn("dbo.Users", "Address", c => c.String());
            AddColumn("dbo.Users", "City", c => c.String(maxLength: 35));
            AddColumn("dbo.Users", "Age", c => c.String());
            AddColumn("dbo.Users", "Phone", c => c.String());
            AddColumn("dbo.Users", "Name", c => c.String(nullable: false, maxLength: 30));
            DropIndex("dbo.Users", new[] { "Username" });
            DropColumn("dbo.Users", "Username");
        }
    }
}
