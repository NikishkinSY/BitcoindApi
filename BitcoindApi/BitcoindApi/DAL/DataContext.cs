using BitcoindApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace BitcoindApi.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<Transaction>().HasKey(s => s.Txid);
            modelBuilder.Entity<HotWallet>().HasKey(s => s.Address);

            //Property Configurations
        }

        public DbSet<IncomeTransaction> Income { get; set; }
        public DbSet<OutcomeTransaction> Outcome { get; set; }
        public DbSet<HotWallet> HotWallets { get; set; }
    }
}
