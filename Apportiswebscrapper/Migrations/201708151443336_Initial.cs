namespace Apportiswebscrapper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Conditions",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        CIDintroduction = c.String(),
                        CIDrisk = c.String(),
                        CIDsymptoms = c.String(),
                        CIDtreatment = c.String(),
                        CIDscreening = c.String(),
                        CIDriskreduction = c.String(),
                        CIDtalking = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Drugcs",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        drgAboutYourTreatment = c.String(),
                        drgAdministeringYourMed = c.String(),
                        drgStorage = c.String(),
                        Whatllow = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Essentials",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        definition = c.String(),
                        causes = c.String(),
                        risk = c.String(),
                        symptoms = c.String(),
                        treatment = c.String(),
                        prevention = c.String(),
                        Searchkeyword = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Healthnews",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        ep_documentBody = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Naturals",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        ep_documentBody = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Procedures",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        definition = c.String(),
                        reasons = c.String(),
                        risk = c.String(),
                        expect = c.String(),
                        call = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
            CreateTable(
                "dbo.Wellnesses",
                c => new
                    {
                        Articleno = c.Int(nullable: false, identity: true),
                        ep_documentBody = c.String(),
                        searchkey = c.String(),
                    })
                .PrimaryKey(t => t.Articleno);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Wellnesses");
            DropTable("dbo.Procedures");
            DropTable("dbo.Naturals");
            DropTable("dbo.Healthnews");
            DropTable("dbo.Essentials");
            DropTable("dbo.Drugcs");
            DropTable("dbo.Conditions");
        }
    }
}
