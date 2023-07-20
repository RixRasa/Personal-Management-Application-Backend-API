using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using TransactionsAPI.Database.Configurations;
using TransactionsAPI.Database.Entities;

namespace TransactionsAPI.Database {
    public class TransDbContext : DbContext{

        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories{ get; set; }

        public DbSet<SplitTransactionEntity> SplitsOfTransaction { get; set; }

        public TransDbContext(DbContextOptions options): base(options) { 
        }

        public TransDbContext() {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

            modelBuilder.Entity<TransactionEntity>(builder => {
                builder.ToTable("transactions");
                builder.HasKey(x => x.Id);
                // definition of columns
                builder.Property(x => x.Id).IsRequired();
                builder.Property(x => x.Beneficiary_name);
                builder.Property(x => x.Date);
                builder.Property(x => x.Direction).HasConversion<string>();
                builder.Property(x => x.Amount);
                builder.Property(x => x.Description);
                builder.Property(x => x.Currency);
                builder.Property(x => x.Mcc);
                builder.Property(x => x.Kind).HasConversion<string>();
                
            });

            modelBuilder.Entity<CategoryEntity>(builder => {
                builder.ToTable("categories");
                builder.HasKey(x => x.Code);
                //definition of columns
                builder.Property(x => x.Code).IsRequired();
                builder.Property(x => x.Parent_code);
                builder.Property(x => x.Name);
            });

            modelBuilder.Entity<SplitTransactionEntity>(builder => {
                builder.ToTable("splitsOfTransaction");
                builder.HasKey(x => x.Id);

                builder.Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
                builder.Property(x => x.catcode);
                builder.Property(x => x.amount);
                

            });
            
            //base.OnModelCreating(modelBuilder);

        }
    }
}
