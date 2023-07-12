using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertBuyOrder_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertBuyOrder = @"
            CREATE PROCEDURE [dbo].[InsertBuyOrder]
            (@BuyOrderID uniqueidentifier, @StockSymbol nvarchar(50), @StockName nvarchar(50), 
             @DateAndTimeOfOrder datetime2, @Quantity bigint, @Price float)
            AS BEGIN
                INSERT INTO [dbo].[BuyOrders](BuyOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price)
                VALUES (@BuyOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price)
            END
            ";

            migrationBuilder.Sql(sp_InsertBuyOrder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertBuyOrder = @"
            DROP PROCEDURE [dbo].[InsertBuyOrder]
            ";
            migrationBuilder.Sql(sp_InsertBuyOrder);
        }
    }
}
