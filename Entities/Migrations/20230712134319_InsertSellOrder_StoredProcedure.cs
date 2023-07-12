using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class InsertSellOrder_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sp_InsertSellOrder = @"
            CREATE PROCEDURE [dbo].[InsertSellOrder]
            (@SellOrderID uniqueidentifier, @StockSymbol nvarchar(50), @StockName nvarchar(50), 
             @DateAndTimeOfOrder datetime2, @Quantity bigint, @Price float)
            AS BEGIN
                INSERT INTO [dbo].[SellOrders](SellOrderID, StockSymbol, StockName, DateAndTimeOfOrder, Quantity, Price)
                VALUES (@SellOrderID, @StockSymbol, @StockName, @DateAndTimeOfOrder, @Quantity, @Price)
            END
            ";

            migrationBuilder.Sql(sp_InsertSellOrder);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sp_InsertSellOrder = @"
            DROP PROCEDURE [dbo].[InsertSellOrder]
            ";
            migrationBuilder.Sql(sp_InsertSellOrder);
        }
    }
}
