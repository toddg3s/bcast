namespace bcast.data.sqlserver.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class resetcode : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Accounts", "ResetCode", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Accounts", "ResetCode");
        }
    }
}
