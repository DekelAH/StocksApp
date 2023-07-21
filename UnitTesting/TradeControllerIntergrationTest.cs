using Fizzler.Systems.HtmlAgilityPack;
using FluentAssertions;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace UnitTesting
{
    public class TradeControllerIntergrationTest : IClassFixture<CustomWebApplicationFactory>
    {
        #region Fields

        private readonly HttpClient _httpClient;

        #endregion

        #region Ctors

        public TradeControllerIntergrationTest(CustomWebApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        #endregion

        #region Test Methods

        [Fact]
        public async Task Index_ReturnToView()
        {
            //Arrange

            //Act
            HttpResponseMessage response = await _httpClient.GetAsync("/Trade/Index/MSFT");

            //Assert
            response.Should().BeSuccessful();

            string responseBody = await response.Content.ReadAsStringAsync();

            HtmlDocument html = new HtmlDocument();
            html.LoadHtml(responseBody);
            var document = html.DocumentNode;

            document.QuerySelectorAll(".price").Should().NotBeNull();
        }

        #endregion
    }
}
