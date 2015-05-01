namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class itemtable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        id = c.String(nullable: false, maxLength: 128),
                        Immediate = c.Boolean(nullable: false),
                        DataType = c.String(),
                        Data = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Items");
        }
    }
}
