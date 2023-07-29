using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;
using ServiceContracts.View_Models;
using StocksApp.Controllers;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {

        #region Ctors

        public CreateOrderActionFilter()
        {
            
        }

        #endregion

        #region Methods

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (context.Controller is TradeController tradeController)
            {
                if (context.ActionArguments["orderRequest"] is IOrderRequest orderRequest)
                {
                    if (!tradeController.ModelState.IsValid)
                    {
                        tradeController.ViewBag.Errors = tradeController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                        StockTrade stock = new StockTrade()
                        {
                            StockSymbol = orderRequest.StockSymbol,
                            StockName = orderRequest.StockName,
                            Price = orderRequest.Price,
                            Quantity = orderRequest.Quantity
                        };

                        context.Result = tradeController.View("Index", stock);
                    }
                    else
                    {
                        await next();
                    }
                }
                else
                {
                    await next();
                }
            }
            else
            {
                await next();
            }
        }

        #endregion
    }
}
