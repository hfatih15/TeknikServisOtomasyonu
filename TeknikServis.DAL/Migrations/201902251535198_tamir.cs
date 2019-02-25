namespace TeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tamir : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Arizalar", "TamirEdildiMi", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Arizalar", "TamirEdildiMi", c => c.Boolean());
        }
    }
}
