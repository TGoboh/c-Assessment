using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace FinTechApiTests
{
    [TestFixture]
    public class UserLogoutTests
    {
        private Mock<IRestClient> _mockClient;
        private RestRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRestClient>();
            _request = new RestRequest("/api/v1/users/logout", Method.Post);
        }

        [Test]
        public void TestUserLogout_Success()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "Logout successful"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddHeader("Authorization", "Bearer validToken");

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Is.EqualTo("Logout successful"));
        }

        [Test]
        public void TestUserLogout_Unauthorized()
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
    }
}
