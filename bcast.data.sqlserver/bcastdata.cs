namespace bcast.data.sqlserver
{
    using bcast.common;
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class bcastdata : DbContext
    {
        // Your context has been configured to use a 'bcastdata' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'bcast.data.sqlserver.bcastdata' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'bcastdata' 
        // connection string in the application configuration file.
        public bcastdata()
            : base("name=bcastdata")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        public virtual DbSet<Account> Account { get; set; }
        public virtual DbSet<Endpoint> Endpoint { get; set; }
        public virtual DbSet<Item> Item { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>()
                .HasKey(x => x.Name);

            modelBuilder.Entity<SecureAccount>().ToTable("SecureAccount");

            modelBuilder.Entity<Endpoint>()
                .HasKey(x => x.Name);
            modelBuilder.Entity<Endpoint>()
                .Property(x => x.Name)
                .HasMaxLength(257);
            modelBuilder.Entity<Endpoint>()
                .Property(x => x.Location)
                .HasMaxLength(500);
            modelBuilder.Entity<Endpoint>()
                .Property(x => x.Enabled)
                .IsRequired();
            modelBuilder.Entity<Endpoint>()
                .Property(x => x.AllCast)
                .IsRequired();
            modelBuilder.Entity<Endpoint>()
                .Property(x => x.Default)
                .IsRequired();

            modelBuilder.Entity<Endpoint>()
                .Ignore(x => x.AccountName)
                .Ignore(x => x.ItemName)
                .Ignore(x => x.LocationUri);

            modelBuilder.Entity<Item>()
                .HasKey(x => x.id);
        }
    }

}