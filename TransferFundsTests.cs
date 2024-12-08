using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace FinTechApiTests
{
    [TestFixture]
    public class TransferFundsTests
    {
        private Mock<IRestClient> _mockClient;
        private RestRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRestClient>();
            _request = new RestRequest("/api/v1/accounts/transfer", Method.Post);
        }

        [Test]
        public void TestTransferFunds_Success()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "Transfer successful"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer validToken");
            _request.AddJsonBody(new
            {
                from_account_id = 12345,
                to_account_id = 67890,
                amount = 100.00
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.EqualTo("Transfer successful"));
        }

        [Test]
        public void TestTransferFunds_BadRequest()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = "Invalid amount or insufficient funds"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer validToken");
            _request.AddJsonBody(new
            {
                from_account_id = 12345,
                to_account_id = 67890,
                amount = -100.00  // Invalid amount
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("Invalid amount or insufficient funds"));
        }
    }
}
