using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TransactionsAPI.Database.Entities;

namespace TransactionsAPI.Database.Configurations {
    public class TransactionEntityTypeConfiguration : IEntityTypeConfiguration<TransactionEntity> {
        public TransactionEntityTypeConfiguration() {
        
        }
        public void Configure(EntityTypeBuilder<TransactionEntity> builder) {
            builder.ToTable("transactions");
            builder.HasKey(x => x.Id);
            // definition of columns
            builder.Property(x => x.Id).IsRequired();
            builder.Property(x => x.Beneficiary_name).HasMaxLength(64);
            builder.Property(x => x.Date); 
            builder.Property(x => x.Direction).HasMaxLength(128);
            builder.Property(x => x.Amount);
            builder.Property(x => x.Description).HasMaxLength(1024);
            builder.Property(x => x.Currency).HasMaxLength(32);
            builder.Property(x => x.Mcc).HasMaxLength(64);
            builder.Property(x => x.Kind).HasConversion<string>().IsRequired();
            
        }
    }
}
