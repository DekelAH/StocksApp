﻿using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Rotativa.AspNetCore;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using ServiceContracts.View_Models;
using StocksApp.OptionsModels;
using StocksApp.ServiceContracts;

namespace StocksApp.Controllers
{
    [Route("trade")]
    public class TradeController : Controller
    {
        #region Fields

        private readonly IFinnhubService _finnHubService;
        private readonly IStocksService _stocksService;
        private readonly IOptions<TradingOptions> _tradingOptions;
        private readonly string? _token;

        #endregion

        #region Ctor

        public TradeController(IFinnhubService finnHubService, IStocksService stocksService, 
            IOptions<TradingOptions> tradingOptions, IConfiguration configuration)
        {
            _finnHubService = finnHubService;
            _stocksService = stocksService;
            _tradingOptions = tradingOptions;
            _token = configuration["FinnhubToken"];
        }

        #endregion

        #region Action Methods

        [Route("[action]/{stockSymbol}")]
        [Route("~/[action]/{stockSymbol}")]
        [HttpGet]
        public async Task<IActionResult> Index(string stockSymbol)
        {
            if (string.IsNullOrEmpty(stockSymbol))
            {
                stockSymbol = "MSFT";
            }
            if (_tradingOptions.Value.DefaultOrderQuantity == null)
            {
                _tradingOptions.Value.DefaultOrderQuantity = "100";
            }

            ViewBag.Token = _token;
            ViewBag.CurrentStockSymbolSearch = stockSymbol;
            Dictionary<string, object>? stockDetailsDic = await _finnHubService.GetStockPriceQuote(stockSymbol);
            Dictionary<string, object>? companyProfileDic = await _finnHubService.GetCompanyProfile(stockSymbol);

            if (stockDetailsDic != null && companyProfileDic != null)
            {
                StockTrade stock = new StockTrade()
                {
                    StockSymbol = companyProfileDic["ticker"].ToString(),
                    StockName = companyProfileDic["name"].ToString(),
                    Price = Convert.ToDouble(stockDetailsDic["c"].ToString()),
                    Quantity = uint.Parse(_tradingOptions.Value.DefaultOrderQuantity)
                };

                return View(stock);
            }

            return View();
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> BuyOrder(BuyOrderRequest buyOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = buyOrderRequest.StockSymbol,
                    StockName = buyOrderRequest.StockName,
                    Price = buyOrderRequest.Price,
                    Quantity = buyOrderRequest.Quantity
                };
                return View("Index", stock);
            }

            buyOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            BuyOrderResponse buyOrderResponse = await _stocksService.CreateBuyOrder(buyOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<IActionResult> SellOrder(SellOrderRequest sellOrderRequest)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                StockTrade stock = new StockTrade()
                {
                    StockSymbol = sellOrderRequest.StockSymbol,
                    StockName = sellOrderRequest.StockName,
                    Price = sellOrderRequest.Price,
                    Quantity = sellOrderRequest.Quantity
                };
                return View("Index", stock);
            }

            sellOrderRequest.DateAndTimeOfOrder = DateTime.Now;
            SellOrderResponse sellOrderResponse = await _stocksService.CreateSellOrder(sellOrderRequest);

            return RedirectToAction("Orders");
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<IActionResult> Orders()
        {
            Orders orders = new()
            {
                BuyOrders = await _stocksService.GetAllBuyOrders(),
                SellOrders = await _stocksService.GetAllSellOrders(),
            };

            return View(orders);
        }

        [Route("[action]")]
        public async Task<IActionResult> OrdersPDF()
        {
            List<IOrderResponse> orders = new();
            orders.AddRange(await _stocksService.GetAllBuyOrders());
            orders.AddRange(await _stocksService.GetAllSellOrders());
            orders = orders.OrderByDescending(order => order.DateAndTimeOfOrder).ToList();

            return new ViewAsPdf("OrdersPDF", orders, ViewData)
            {
                PageMargins = new Rotativa.AspNetCore.Options.Margins() {Top = 20, Bottom = 20, Left = 20, Right = 20 },
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape
            };
        }

        #endregion
    }
}
