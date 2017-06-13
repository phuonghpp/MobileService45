namespace MobileService45.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class OracleMobileDB : DbContext
    {
        public OracleMobileDB()
            : base("OracleMobileDB")
        {
        }
        

        public virtual DbSet<LOG> LOGS { get; set; }
        public virtual DbSet<USER_GROUP> USER_GROUP { get; set; }
        public virtual DbSet<USER> USERS { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LOG>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LOG>()
                .Property(e => e.USERID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<LOG>()
                .Property(e => e.MENUID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<USER_GROUP>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<USER_GROUP>()
                .Property(e => e.USERID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<USER_GROUP>()
                .Property(e => e.GROUPID)
                .HasPrecision(38, 0);

            modelBuilder.Entity<USER>()
                .Property(e => e.ID)
                .HasPrecision(38, 0);
        }
    }
}
