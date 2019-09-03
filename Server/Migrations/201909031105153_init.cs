namespace Server.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Clubs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SportId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sports", t => t.SportId, cascadeDelete: true)
                .Index(t => t.SportId);
            
            CreateTable(
                "dbo.Members",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        LastName = c.String(),
                        FirstName = c.String(),
                        Age = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sports",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MemberClubs",
                c => new
                    {
                        Member_Id = c.Int(nullable: false),
                        Club_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Member_Id, t.Club_Id })
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .ForeignKey("dbo.Clubs", t => t.Club_Id, cascadeDelete: true)
                .Index(t => t.Member_Id)
                .Index(t => t.Club_Id);
            
            CreateTable(
                "dbo.SportMembers",
                c => new
                    {
                        Sport_Id = c.Int(nullable: false),
                        Member_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Sport_Id, t.Member_Id })
                .ForeignKey("dbo.Sports", t => t.Sport_Id, cascadeDelete: true)
                .ForeignKey("dbo.Members", t => t.Member_Id, cascadeDelete: true)
                .Index(t => t.Sport_Id)
                .Index(t => t.Member_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SportMembers", "Member_Id", "dbo.Members");
            DropForeignKey("dbo.SportMembers", "Sport_Id", "dbo.Sports");
            DropForeignKey("dbo.Clubs", "SportId", "dbo.Sports");
            DropForeignKey("dbo.MemberClubs", "Club_Id", "dbo.Clubs");
            DropForeignKey("dbo.MemberClubs", "Member_Id", "dbo.Members");
            DropIndex("dbo.SportMembers", new[] { "Member_Id" });
            DropIndex("dbo.SportMembers", new[] { "Sport_Id" });
            DropIndex("dbo.MemberClubs", new[] { "Club_Id" });
            DropIndex("dbo.MemberClubs", new[] { "Member_Id" });
            DropIndex("dbo.Clubs", new[] { "SportId" });
            DropTable("dbo.SportMembers");
            DropTable("dbo.MemberClubs");
            DropTable("dbo.Sports");
            DropTable("dbo.Members");
            DropTable("dbo.Clubs");
        }
    }
}
