using Microsoft.EntityFrameworkCore;

namespace WalletDomain
{
    public class WalletDbContext : DbContext
    {
        public WalletDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }


        public DbSet<Wallet> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Wallet>(b =>
            {
                b.HasIndex(e => e.Account).IsUnique();
                b.Property(e => e.Account).IsRequired();
                b.Property(e => e.Account).HasMaxLength(20);
                b.Property(e => e.Amount).IsRequired();
                b.Property(e => e.Direction).IsRequired();

            });
        }
    }
}