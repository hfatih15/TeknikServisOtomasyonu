namespace TeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class urunAdi : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Arizalar", "UrunAdi", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Arizalar", "UrunAdi");
        }
    }
}
