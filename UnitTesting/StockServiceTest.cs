using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using Services;
using Xunit.Abstractions;

namespace UnitTesting
{
    public class StockServiceTest
    {
        #region Fields

        private readonly IStocksService _stocksService;
        private readonly ITestOutputHelper _testOutputHelper;

        #endregion

        #region Ctors

        public StockServiceTest(ITestOutputHelper testOutputHelper)
        {
            _stocksService = new StocksService();
            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region Test Methods

        #region CreateBuyOrder

        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async void CreateBuyOrder_Null_BuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderQuantity_Under_Min()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 0,
                Price = 1000
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderQuantity_Above_Max()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 100001,
                Price = 1000
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderPrice_Under_Min()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 0
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderPrice_Above_Max()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 10001
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_NullBuyOrderStockSymbol()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = null,
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 100
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        //it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderDateAndTimeOfOrder_OlderValue()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPL",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("1991-02-04"),
                Quantity = 1000,
                Price = 100
            };

            _testOutputHelper.WriteLine($"{buyOrderRequest.DateAndTimeOfOrder}");
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //If you supply all valid values, it should be successful and return
        //an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidDetails()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPL",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2001-02-04"),
                Quantity = 1000,
                Price = 100
            };

            //Act
            BuyOrderResponse? buyOrderResponseFromAdd = await _stocksService.CreateBuyOrder(buyOrderRequest);
            _testOutputHelper.WriteLine("From Add:");
            _testOutputHelper.WriteLine(buyOrderResponseFromAdd.ToString());
            List<BuyOrderResponse> buyOrderResponseList = await _stocksService.GetAllBuyOrders();
            foreach (var item in buyOrderResponseList)
            {
                _testOutputHelper.WriteLine("From List");
                _testOutputHelper.WriteLine(item.ToString());
            }

            //Assert
            Assert.True(buyOrderResponseFromAdd.BuyOrderID != Guid.Empty);
            Assert.Contains(buyOrderResponseFromAdd, buyOrderResponseList);
        }

        #endregion

        #region CreateSellOrder

        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async void CreateSellOrder_Null_SellOrderRequest()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderQuantity_Under_Min()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 0,
                Price = 1000
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderQuantity_Above_Max()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 100001,
                Price = 1000
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderPrice_Under_Min()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 0
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderPrice_Above_Max()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPLE",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 10001
            };

            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_NullSellOrderStockSymbol()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = null,
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2000-02-04"),
                Quantity = 1000,
                Price = 100
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        //it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderDateAndTimeOfOrder_OlderValue()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPL",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("1999-02-04"),
                Quantity = 1000,
                Price = 100
            };
            //Assert
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                //Act
                await _stocksService.CreateSellOrder(sellOrderRequest);
            });
        }

        //If you supply all valid values, it should be successful and return
        //an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidDetails()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = new SellOrderRequest()
            {
                StockSymbol = "APPL",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("2001-02-04"),
                Quantity = 1000,
                Price = 100
            };

            //Act
            SellOrderResponse? sellOrderResponseFromAdd = await _stocksService.CreateSellOrder(sellOrderRequest);
            _testOutputHelper.WriteLine("From Add:");
            _testOutputHelper.WriteLine(sellOrderResponseFromAdd.ToString());
            List<SellOrderResponse> sellOrderResponseList = await _stocksService.GetAllSellOrders();
            foreach (var item in sellOrderResponseList)
            {
                _testOutputHelper.WriteLine("From List");
                _testOutputHelper.WriteLine(item.ToString());
            }

            //Assert
            Assert.True(sellOrderResponseFromAdd.SellOrderID != Guid.Empty);
            Assert.Contains(sellOrderResponseFromAdd, sellOrderResponseList);
        }

        #endregion

        #region GetAllBuyOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_ListEmpty()
        {
            //Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksService.GetAllBuyOrders();

            //Assert
            Assert.Empty(buyOrderResponses);
        }

        //When you first add few buy orders using CreateBuyOrder() method;
        //and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest01 = new BuyOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 3,
                Price = 235
            };
            BuyOrderRequest buyOrderRequest02 = new BuyOrderRequest()
            {
                StockSymbol = "EA",
                StockName = "Electronic Arts",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 6,
                Price = 435
            };
            BuyOrderRequest buyOrderRequest03 = new BuyOrderRequest()
            {
                StockSymbol = "SGAMY:OTCPK",
                StockName = "Sega Sammy Holdings",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 2,
                Price = 135
            };

            List<BuyOrderRequest> buyOrderRequests = new List<BuyOrderRequest>()
            {
                buyOrderRequest01, buyOrderRequest02, buyOrderRequest03
            };
            List<BuyOrderResponse> buyOrderResponsesFromAdd = new List<BuyOrderResponse>();

            foreach (var buyOrderRequest in buyOrderRequests)
            {
                buyOrderResponsesFromAdd.Add(await _stocksService.CreateBuyOrder(buyOrderRequest));
            }
            _testOutputHelper.WriteLine("Expected:");
            foreach (var buyOrderResponseFromAdd in buyOrderResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(buyOrderResponseFromAdd.ToString());
            }

            //Act
            List<BuyOrderResponse> buyOrderResponsesFromGet = await _stocksService.GetAllBuyOrders();
            _testOutputHelper.WriteLine("Actual:");
            foreach (var buyOrderResponseFromGet in buyOrderResponsesFromGet)
            {
                _testOutputHelper.WriteLine(buyOrderResponseFromGet.ToString());
            }

            //Assert
            foreach (var buyOrderResponseFromAdd in buyOrderResponsesFromAdd)
            {
                Assert.Contains(buyOrderResponseFromAdd, buyOrderResponsesFromGet);
            }

        }

        #endregion

        #region GetAllSellOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_ListEmpty()
        {
            //Act
            List<SellOrderResponse> sellOrderResponses = await _stocksService.GetAllSellOrders();

            //Assert
            Assert.Empty(sellOrderResponses);
        }

        //When you first add few sell orders using CreateSellOrder() method;
        //and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders()
        {
            //Arrange
            SellOrderRequest sellOrderRequest01 = new SellOrderRequest()
            {
                StockSymbol = "MSFT",
                StockName = "Microsoft",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 3,
                Price = 235
            };
            SellOrderRequest sellOrderRequest02 = new SellOrderRequest()
            {
                StockSymbol = "EA",
                StockName = "Electronic Arts",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 6,
                Price = 435
            };
            SellOrderRequest sellOrderRequest03 = new SellOrderRequest()
            {
                StockSymbol = "SGAMY:OTCPK",
                StockName = "Sega Sammy Holdings",
                DateAndTimeOfOrder = DateTime.Parse("2023-02-06"),
                Quantity = 2,
                Price = 135
            };

            List<SellOrderRequest> sellOrderRequests = new List<SellOrderRequest>()
            {
                sellOrderRequest01, sellOrderRequest02, sellOrderRequest03
            };
            List<SellOrderResponse> sellOrderResponsesFromAdd = new List<SellOrderResponse>();

            foreach (var sellOrderRequest in sellOrderRequests)
            {
                sellOrderResponsesFromAdd.Add(await _stocksService.CreateSellOrder(sellOrderRequest));
            }
            _testOutputHelper.WriteLine("Expected:");
            foreach (var sellOrderResponseFromAdd in sellOrderResponsesFromAdd)
            {
                _testOutputHelper.WriteLine(sellOrderResponseFromAdd.ToString());
            }

            //Act
            List<SellOrderResponse> sellOrderResponsesFromGet = await _stocksService.GetAllSellOrders();
            _testOutputHelper.WriteLine("Actual:");
            foreach (var sellOrderResponseFromGet in sellOrderResponsesFromGet)
            {
                _testOutputHelper.WriteLine(sellOrderResponseFromGet.ToString());
            }

            //Assert
            foreach (var sellOrderResponseFromAdd in sellOrderResponsesFromAdd)
            {
                Assert.Contains(sellOrderResponseFromAdd, sellOrderResponsesFromGet);
            }

        }

        #endregion

        #endregion
    }
}
