namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EntityName : DbMigration
    {
        public override void Up()
        {
            DropPrimaryKey("dbo.Endpoints");
            AddColumn("dbo.Endpoints", "FullName", c => c.String(nullable: false, maxLength: 257));
            AddPrimaryKey("dbo.Endpoints", "FullName");
            DropColumn("dbo.Endpoints", "Name");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Endpoints", "Name", c => c.String(nullable: false, maxLength: 257));
            DropPrimaryKey("dbo.Endpoints");
            DropColumn("dbo.Endpoints", "FullName");
            AddPrimaryKey("dbo.Endpoints", "Name");
        }
    }
}
