using Bitcoind.Core.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bitcoind.Core.DAL
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Write Fluent API configurations here
            modelBuilder.Entity<Transaction>().HasKey(s => s.Txid);
            modelBuilder.Entity<HotWallet>().HasKey(s => s.Address);
            modelBuilder.Entity<Transaction>().HasOne<HotWallet>().WithMany().HasForeignKey(x => x.Wallet);
            //Property Configurations
        }

        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<HotWallet> HotWallets { get; set; }
    }
}
