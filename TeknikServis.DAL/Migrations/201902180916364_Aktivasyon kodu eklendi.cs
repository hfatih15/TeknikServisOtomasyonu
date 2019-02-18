namespace TeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Aktivasyonkodueklendi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ActivationCode", c => c.String());
            AddColumn("dbo.AspNetUsers", "AvatarPath", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "AvatarPath");
            DropColumn("dbo.AspNetUsers", "ActivationCode");
        }
    }
}
