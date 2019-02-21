namespace TeknikServis.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ariza : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Arizalar",
                c => new
                    {
                        ArizaId = c.Int(nullable: false, identity: true),
                        MusteriId = c.String(nullable: false),
                        OperatorId = c.String(),
                        TeknisyenId = c.String(),
                        ArizaOlusturmaTarihi = c.DateTime(nullable: false),
                        ArizaKabulTarihi = c.DateTime(),
                        ArizaBitisTarihi = c.DateTime(),
                        TamirEdildiMi = c.Boolean(),
                        ArizaKabulEdildiMi = c.Boolean(),
                        ArizaTeknisyeneAtandiMi = c.Boolean(),
                        GarantiDurumu = c.Boolean(nullable: false),
                        MusteriYorumu = c.String(nullable: false),
                        TeknisyenYorumu = c.String(),
                        FaturaResmi = c.String(),
                        UrunResmi = c.String(nullable: false),
                        Adres = c.String(nullable: false),
                        UrunDurumu = c.Int(),
                        UrunTipi = c.Int(nullable: false),
                        SehirAdi = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ArizaId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Arizalar");
        }
    }
}
