﻿using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StocksService : IStocksService
    {
        #region Methods

        public Task<BuyOrderResponse> CreateBuyOrder(BuyOrderRequest? buyOrderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<SellOrderResponse> CreateSellOrder(SellOrderRequest? sellOrderRequest)
        {
            throw new NotImplementedException();
        }

        public Task<List<BuyOrderResponse>> GetBuyOrders()
        {
            throw new NotImplementedException();
        }

        public Task<List<SellOrderResponse>> GetSellOrders()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}