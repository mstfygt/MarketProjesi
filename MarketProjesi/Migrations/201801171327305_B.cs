namespace MarketProjesi.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class B : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Kategoriler",
                c => new
                    {
                        KategoriId = c.Int(nullable: false, identity: true),
                        KategoriAdı = c.String(name: "Kategori Adı", nullable: false, maxLength: 25, unicode: false),
                        Aciklama = c.String(),
                        KDVOran = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.KategoriId);
            
            CreateTable(
                "dbo.Ürünler",
                c => new
                    {
                        UrunId = c.Int(nullable: false, identity: true),
                        UrunAdi = c.String(nullable: false, maxLength: 40),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Stok = c.Short(nullable: false),
                        SeriNo = c.Long(nullable: false),
                        Fotograf = c.Binary(),
                        KategoriId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UrunId)
                .ForeignKey("dbo.Kategoriler", t => t.KategoriId, cascadeDelete: true)
                .Index(t => t.UrunAdi, unique: true)
                .Index(t => t.KategoriId);
            
            CreateTable(
                "dbo.SiparisDetaylar",
                c => new
                    {
                        SiparisId = c.Int(nullable: false),
                        UrunId = c.Int(nullable: false),
                        Adet = c.Int(nullable: false),
                        Fiyat = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Indirim = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => new { t.SiparisId, t.UrunId })
                .ForeignKey("dbo.Siparişler", t => t.SiparisId, cascadeDelete: true)
                .ForeignKey("dbo.Ürünler", t => t.UrunId, cascadeDelete: true)
                .Index(t => t.SiparisId)
                .Index(t => t.UrunId);
            
            CreateTable(
                "dbo.Siparişler",
                c => new
                    {
                        SiparisId = c.Int(nullable: false, identity: true),
                        SiparisTarihi = c.DateTime(nullable: false),
                        TeslimTarihi = c.DateTime(),
                    })
                .PrimaryKey(t => t.SiparisId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SiparisDetaylar", "UrunId", "dbo.Ürünler");
            DropForeignKey("dbo.SiparisDetaylar", "SiparisId", "dbo.Siparişler");
            DropForeignKey("dbo.Ürünler", "KategoriId", "dbo.Kategoriler");
            DropIndex("dbo.SiparisDetaylar", new[] { "UrunId" });
            DropIndex("dbo.SiparisDetaylar", new[] { "SiparisId" });
            DropIndex("dbo.Ürünler", new[] { "KategoriId" });
            DropIndex("dbo.Ürünler", new[] { "UrunAdi" });
            DropTable("dbo.Siparişler");
            DropTable("dbo.SiparisDetaylar");
            DropTable("dbo.Ürünler");
            DropTable("dbo.Kategoriler");
        }
    }
}
