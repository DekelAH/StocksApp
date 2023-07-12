using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    public partial class GetBuyOrders_StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string Sp_GetAllBuyOrders = @"
            CREATE PROCEDURE [dbo].[GetAllBuyOrders] 
            AS BEGIN
            SELECT BuyOrderID, StockSymbol, StockName, DateAndTimeOfOrder,Quantity, Price
            FROM [dbo].[BuyOrders]
            
            END
            ";

            migrationBuilder.Sql(Sp_GetAllBuyOrders);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string Sp_GetAllBuyOrders = @"
            DROP PROCEDURE [dbo].[GetAllBuyOrders]
            ";

            migrationBuilder.Sql(Sp_GetAllBuyOrders);
        }
    }
}
