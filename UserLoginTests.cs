using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace FinTechApiTests
{
    [TestFixture]
    public class UserLoginTests
    {
        private Mock<IRestClient> _mockClient;
        private RestRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRestClient>();
            _request = new RestRequest("/api/v1/users/login", Method.Post);
        }

        [Test]
        public void TestUserLogin_Success()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.OK,
                Content = "{\"token\": \"jwt-token-here\"}"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "validuser",
                password = "validpassword"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(response.Content, Does.Contain("jwt-token-here"));
        }

        [Test]
        public void TestUserLogin_Unauthorized()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = "Invalid credentials"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "invaliduser",
                password = "wrongpassword"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
            Assert.That(response.Content, Is.EqualTo("Invalid credentials"));
        }

        [Test]
        public void TestUserLogin_BadRequest()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = "Missing required fields"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "",
                password = "validpassword"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("Missing required fields"));
        }
    }
}
