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
                        Locked = c.Boolean(nullable: false),
                        Password = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Endpoints");
            DropTable("dbo.Accounts");
        }
    }
}
