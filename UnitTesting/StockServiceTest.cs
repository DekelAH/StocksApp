using AutoFixture;
using Entities.Models;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using RepositoryContracts;
using Serilog;
using ServiceContracts.Contracts;
using ServiceContracts.DTO;
using Services;
using StocksApp.Controllers;
using Xunit.Abstractions;

namespace UnitTesting
{
    public class StockServiceTest
    {
        #region Fields

        private readonly IStocksGetterService _stocksGetterService;
        private readonly IStocksCreaterService _stocksCreaterService;
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IStocksRepository _stocksRepository;
        private readonly IFixture _autoFixture;
        private readonly Mock<IStocksRepository> _stocksRepositoryMock;

        #endregion

        #region Ctors

        public StockServiceTest(ITestOutputHelper testOutputHelper)
        {
            _autoFixture = new Fixture();
            _stocksRepositoryMock = new Mock<IStocksRepository>();
            _stocksRepository = _stocksRepositoryMock.Object;

            var loggerGetter = new Mock<ILogger<StocksGetterService>>();
            var loggerCreater = new Mock<ILogger<StocksCreaterService>>();

            var diagnosticContext = new Mock<IDiagnosticContext>();

            _stocksGetterService = new StocksGetterService(_stocksRepository, loggerGetter.Object, diagnosticContext.Object);
            _stocksCreaterService = new StocksCreaterService(_stocksRepository, loggerCreater.Object, diagnosticContext.Object);

            _testOutputHelper = testOutputHelper;
        }

        #endregion

        #region Test Methods

        #region CreateBuyOrder

        //When you supply BuyOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = null;

            //Mock
            BuyOrder buyOrder = _autoFixture.Build<BuyOrder>().Create();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply buyOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderQuantity_Under_Min_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(0))
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderQuantity_Above_Max_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderPrice_Under_Min_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply buyOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderPrice_Above_Max_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.Price, 10001.0)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_NullBuyOrderStockSymbol_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        //it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async void CreateBuyOrder_BuyOrderDateAndTimeOfOrder_OlderValue_ToBeArgumentException()
        {
            //Arrange
            BuyOrderRequest? buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1991-05-05"))
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return
        //an object of BuyOrderResponse type with auto-generated BuyOrderID (guid).
        [Fact]
        public async void CreateBuyOrder_ValidDetails_ToBeSuccessful()
        {
            //Arrange
            BuyOrderRequest buyOrderRequest = _autoFixture.Build<BuyOrderRequest>()
                .With(temp => temp.StockSymbol, "APPL")
                .With(temp => temp.StockName, "APPLE")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create();

            //Mock
            BuyOrder buyOrder = buyOrderRequest.ToBuyOrder();
            _stocksRepositoryMock.Setup(method => method.CreateBuyOrder(It.IsAny<BuyOrder>())).ReturnsAsync(buyOrder);

            //Act
            BuyOrderResponse? buyOrderResponseFromService = await _stocksCreaterService.CreateBuyOrder(buyOrderRequest);

            //Assert
            buyOrder.BuyOrderID = buyOrderResponseFromService.BuyOrderID;
            buyOrderResponseFromService.BuyOrderID.Should().NotBe(Guid.Empty);
            buyOrder.ToBuyOrderResponse().Should().BeEquivalentTo(buyOrderResponseFromService);
        }

        #endregion

        #region CreateSellOrder

        //When you supply SellOrderRequest as null, it should throw ArgumentNullException.
        [Fact]
        public async void CreateSellOrder_Null_SellOrderRequest_ToBeArgumentNullException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = null;

            //Mock
            SellOrder sellOrder = _autoFixture.Build<SellOrder>().Create();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentNullException>();
        }

        //When you supply sellOrderQuantity as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderQuantity_Under_Min_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(0))
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderQuantity as 100001 (as per the specification, maximum is 100000), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderQuantity_Above_Max_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.Quantity, Convert.ToUInt32(100001))
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 0 (as per the specification, minimum is 1), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderPrice_Under_Min_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 0)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply sellOrderPrice as 10001 (as per the specification, maximum is 10000), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderPrice_Above_Max_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.Price, 10001.0)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply stock symbol=null (as per the specification, stock symbol can't be null), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_NullSellOrderStockSymbol_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, null as string)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //When you supply dateAndTimeOfOrder as "1999-12-31" (YYYY-MM-DD) - (as per the specification,
        //it should be equal or newer date than 2000-01-01), it should throw ArgumentException.
        [Fact]
        public async void CreateSellOrder_SellOrderDateAndTimeOfOrder_OlderValue_ToBeArgumentException()
        {
            //Arrange
            SellOrderRequest? sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.DateAndTimeOfOrder, Convert.ToDateTime("1991-05-05"))
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Assert
            Func<Task> action = async () =>
            {
                //Act
                await _stocksCreaterService.CreateSellOrder(sellOrderRequest);
            };
            await action.Should().ThrowAsync<ArgumentException>();
        }

        //If you supply all valid values, it should be successful and return
        //an object of SellOrderResponse type with auto-generated SellOrderID (guid).
        [Fact]
        public async void CreateSellOrder_ValidDetails_ToBeSuccessful()
        {
            //Arrange
            SellOrderRequest sellOrderRequest = _autoFixture.Build<SellOrderRequest>()
                .With(temp => temp.StockSymbol, "APPL")
                .With(temp => temp.StockName, "APPLE")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create();

            //Mock
            SellOrder sellOrder = sellOrderRequest.ToSellOrder();
            _stocksRepositoryMock.Setup(method => method.CreateSellOrder(It.IsAny<SellOrder>())).ReturnsAsync(sellOrder);

            //Act
            SellOrderResponse? sellOrderResponseFromService = await _stocksCreaterService.CreateSellOrder(sellOrderRequest);

            //Assert
            sellOrder.SellOrderID = sellOrderResponseFromService.SellOrderID;
            sellOrderResponseFromService.SellOrderID.Should().NotBe(Guid.Empty);
            sellOrder.ToSellOrderResponse().Should().BeEquivalentTo(sellOrderResponseFromService);
        }

        #endregion

        #region GetAllBuyOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllBuyOrders_ToBeEmpty()
        {
            //Arrange
            List<BuyOrder> buyOrdersEmptyList = new List<BuyOrder>();

            //Mock
            _stocksRepositoryMock.Setup(method => method.GetBuyOrders()).ReturnsAsync(buyOrdersEmptyList);
            //Act
            List<BuyOrderResponse> buyOrderResponses = await _stocksGetterService.GetAllBuyOrders();

            //Assert
            buyOrderResponses.Should().BeEmpty();
        }

        //When you first add few buy orders using CreateBuyOrder() method;
        //and then invoke GetAllBuyOrders() method; the returned list should contain all the same buy orders.
        [Fact]
        public async void GetAllBuyOrders_AddFewBuyOrders_ToBeSuccessful()
        {
            //Arrange
            List<BuyOrder> buyOrders = new List<BuyOrder>()
            {
             _autoFixture.Build<BuyOrder>()
                .With(temp => temp.StockSymbol, "APPL")
                .With(temp => temp.StockName, "APPLE")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create(),
             _autoFixture.Build<BuyOrder>()
                .With(temp => temp.StockSymbol, "MSFT")
                .With(temp => temp.StockName, "Microsoft")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create(),
             _autoFixture.Build<BuyOrder>()
                .With(temp => temp.StockSymbol, "JJ")
                .With(temp => temp.StockName, "JohnsonAndJohnson")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create()
            };


            List<BuyOrderResponse> buyOrderResponses = buyOrders.Select(buyOrder => buyOrder.ToBuyOrderResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            foreach (var buyOrderResponse in buyOrderResponses)
            {
                _testOutputHelper.WriteLine(buyOrderResponse.ToString());
            }

            //Mock
            _stocksRepositoryMock.Setup(method => method.GetBuyOrders()).ReturnsAsync(buyOrders);

            //Act
            List<BuyOrderResponse> buyOrderResponsesFromService = await _stocksGetterService.GetAllBuyOrders();
            _testOutputHelper.WriteLine("Actual:");
            foreach (var buyOrderResponse in buyOrderResponsesFromService)
            {
                _testOutputHelper.WriteLine(buyOrderResponse.ToString());
            }

            //Assert
            buyOrderResponsesFromService.Should().BeEquivalentTo(buyOrderResponses);

        }

        #endregion

        #region GetAllSellOrders

        //When you invoke this method, by default, the returned list should be empty.
        [Fact]
        public async void GetAllSellOrders_ListEmpty()
        {
            //Arrange
            List<SellOrder> sellOrdersEmptyList = new List<SellOrder>();

            //Mock
            _stocksRepositoryMock.Setup(method => method.GetSellOrders()).ReturnsAsync(sellOrdersEmptyList);
            //Act
            List<SellOrderResponse> sellOrderResponses = await _stocksGetterService.GetAllSellOrders();

            //Assert
            sellOrderResponses.Should().BeEmpty();
        }

        //When you first add few sell orders using CreateSellOrder() method;
        //and then invoke GetAllSellOrders() method; the returned list should contain all the same sell orders.
        [Fact]
        public async void GetAllSellOrders_AddFewSellOrders()
        {
            //Arrange
            List<SellOrder> sellOrders = new List<SellOrder>()
            {
             _autoFixture.Build<SellOrder>()
                .With(temp => temp.StockSymbol, "APPL")
                .With(temp => temp.StockName, "APPLE")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create(),
             _autoFixture.Build<SellOrder>()
                .With(temp => temp.StockSymbol, "MSFT")
                .With(temp => temp.StockName, "Microsoft")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create(),
             _autoFixture.Build<SellOrder>()
                .With(temp => temp.StockSymbol, "JJ")
                .With(temp => temp.StockName, "JohnsonAndJohnson")
                .With(temp => temp.DateAndTimeOfOrder, DateTime.Parse("2001-02-04"))
                .With(temp => temp.Quantity, Convert.ToUInt32(1000))
                .With(temp => temp.Price, 100)
                .Create()
            };

            List<SellOrderResponse> sellOrderResponses = sellOrders.Select(sellOrder => sellOrder.ToSellOrderResponse()).ToList();

            _testOutputHelper.WriteLine("Expected:");
            foreach (var sellOrderResponse in sellOrderResponses)
            {
                _testOutputHelper.WriteLine(sellOrderResponse.ToString());
            }

            //Mock
            _stocksRepositoryMock.Setup(method => method.GetSellOrders()).ReturnsAsync(sellOrders);

            //Act
            List<SellOrderResponse> sellOrderResponsesFromService = await _stocksGetterService.GetAllSellOrders();
            _testOutputHelper.WriteLine("Actual:");
            foreach (var sellOrderResponse in sellOrderResponsesFromService)
            {
                _testOutputHelper.WriteLine(sellOrderResponse.ToString());
            }

            //Assert
            sellOrderResponsesFromService.Should().BeEquivalentTo(sellOrderResponses);
        }

        #endregion

        #endregion
    }
}
