namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Email = c.String(),
                        Locked = c.Boolean(nullable: false),
                        ResetCode = c.String(),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Endpoints",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 257),
                        Type = c.Int(nullable: false),
                        Location = c.String(maxLength: 500),
                        Enabled = c.Boolean(nullable: false),
                        AllCast = c.Boolean(nullable: false),
                        Default = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Name);
            
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        DataType = c.String(),
                        Data = c.String(),
                        Immediate = c.Boolean(),
                        Source = c.String(),
                        ContentType = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                        Sec_id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.SecurityContexts", t => t.Sec_id)
                .Index(t => t.Sec_id);
            
            CreateTable(
                "dbo.SecurityContexts",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        account = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.SecureAccount",
                c => new
                    {
                        Name = c.String(nullable: false, maxLength: 128),
                        Password = c.String(),
                    })
                .PrimaryKey(t => t.Name)
                .ForeignKey("dbo.Accounts", t => t.Name)
                .Index(t => t.Name);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SecureAccount", "Name", "dbo.Accounts");
            DropForeignKey("dbo.Items", "Sec_id", "dbo.SecurityContexts");
            DropIndex("dbo.SecureAccount", new[] { "Name" });
            DropIndex("dbo.Items", new[] { "Sec_id" });
            DropTable("dbo.SecureAccount");
            DropTable("dbo.SecurityContexts");
            DropTable("dbo.Items");
            DropTable("dbo.Endpoints");
            DropTable("dbo.Accounts");
        }
    }
}
