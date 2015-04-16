namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class secureaccount : DbMigration
    {
        public override void Up()
        {
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
            
            DropColumn("dbo.Accounts", "Password");
            DropColumn("dbo.Accounts", "Discriminator");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Accounts", "Discriminator", c => c.String(nullable: false, maxLength: 128));
            AddColumn("dbo.Accounts", "Password", c => c.String());
            DropForeignKey("dbo.SecureAccount", "Name", "dbo.Accounts");
            DropIndex("dbo.SecureAccount", new[] { "Name" });
            DropTable("dbo.SecureAccount");
        }
    }
}
