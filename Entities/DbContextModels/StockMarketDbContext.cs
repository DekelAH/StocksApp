using Entities.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Entities.DbContextModels
{
    public class StockMarketDbContext : DbContext
    {
        #region Properties

        public DbSet<BuyOrder> BuyOrders { get; set; }
        public DbSet<SellOrder> SellOrders { get; set; }

        #endregion

        #region Ctors

        public StockMarketDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions) { }

        #endregion

        #region Methods

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BuyOrder>().ToTable("BuyOrders");
            modelBuilder.Entity<SellOrder>().ToTable("SellOrders");
        }

        public List<BuyOrder> Sp_GetAllBuyOrders()
        {
            return BuyOrders.FromSqlRaw("EXECUTE [dbo].[GetAllBuyOrders]").ToList();
        }

        public List<SellOrder> Sp_GetAllSellOrders()
        {
            return SellOrders.FromSqlRaw("EXECUTE [dbo].[GetAllSellOrders]").ToList();
        }

        public int Sp_InsertBuyOrder(BuyOrder newBuyOrder)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@BuyOrderID", newBuyOrder.BuyOrderID),
                new SqlParameter("@StockSymbol", newBuyOrder.StockSymbol),
                new SqlParameter("@StockName", newBuyOrder.StockName),
                new SqlParameter("@DateAndTimeOfOrder", newBuyOrder.DateAndTimeOfOrder),
                new SqlParameter("@Quantity", Convert.ToInt32(newBuyOrder.Quantity)),
                new SqlParameter("@Price", newBuyOrder.Price),
            };

            int rowAffected = Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertBuyOrder] " +
                "@BuyOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price", sqlParameters);

            return rowAffected;
        }

        public int Sp_InsertSellOrder(SellOrder newSellOrder)
        {
            SqlParameter[] sqlParameters = new SqlParameter[]
            {
                new SqlParameter("@SellOrderID", newSellOrder.SellOrderID),
                new SqlParameter("@StockSymbol", newSellOrder.StockSymbol),
                new SqlParameter("@StockName", newSellOrder.StockName),
                new SqlParameter("@DateAndTimeOfOrder", newSellOrder.DateAndTimeOfOrder),
                new SqlParameter("@Quantity", Convert.ToInt32(newSellOrder.Quantity)),
                new SqlParameter("@Price", newSellOrder.Price),
            };

            int rowAffected = Database.ExecuteSqlRaw("EXECUTE [dbo].[InsertSellOrder] " +
                "@SellOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price", sqlParameters);

            return rowAffected;
        }

        #endregion
    }
}
