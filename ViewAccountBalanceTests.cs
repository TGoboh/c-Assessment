using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace FinTechApiTests
{
    [TestFixture]
    public class ViewAccountBalanceTests
    {
        private Mock<IRestClient> _mockClient;
        private RestRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRestClient>();
            _request = new RestRequest("/api/v1/accounts/{account_id}/balance", Method.Get);
        }

        [Test]
        public void TestViewAccountBalance_Success()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"balance\": 1000.00}"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer validToken");

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("\"balance\": 1000.00"));
        }

        [Test]
        public void TestViewAccountBalance_Unauthorized()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = "Missing or invalid token"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer invalidToken");

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.Content, Is.EqualTo("Missing or invalid token"));
        }

        [Test]
        public void TestViewAccountBalance_NotFound()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.NotFound,
                Content = "Account does not exist"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer validToken");

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
            Assert.That(response.Content, Is.EqualTo("Account does not exist"));
        }
    }
}
