namespace TeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ariza : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "SehirAdi", c => c.Int());
            AddColumn("dbo.AspNetUsers", "Adres", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Adres");
            DropColumn("dbo.AspNetUsers", "SehirAdi");
        }
    }
}
