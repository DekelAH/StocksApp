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
        public void CreateBuyOrder_Null_BuyOrderRequest()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Assert
            Assert.ThrowsAsync<ArgumentNullException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });

        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantity_Under_Min()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderQuantity_Above_Max()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPrice_Under_Min()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderPrice_Above_Max()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_NullBuyOrderStockSymbol()
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
            Assert.ThrowsAsync<ArgumentException>(() =>
            {
                //Act
                return _stocksService.CreateBuyOrder(buyOrderRequest);
            });
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        //it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public void CreateBuyOrder_BuyOrderDateAndTimeOfOrder_OlderValue()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = new BuyOrderRequest()
            {
                StockSymbol = "APPL",
                StockName = "Test",
                DateAndTimeOfOrder = DateTime.Parse("1999-02-04"),
                Quantity = 1000,
                Price = 100
            };
            //Assert
            Assert.ThrowsAsync<ArgumentException>(() =>
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
            List<BuyOrderResponse> buyOrderResponseList = await _stocksService.GetBuyOrders();

            //Assert
            Assert.True(buyOrderResponseFromAdd != null);
            Assert.Contains(buyOrderResponseFromAdd, buyOrderResponseList);
        }

        #endregion

        #endregion
    }
}
