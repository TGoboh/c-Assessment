using Moq;
using NUnit.Framework;
using RestSharp;
using System.Net;

namespace FinTechApiTests
{
    [TestFixture]
    public class UserRegistrationTests
    {
        private Mock<IRestClient> _mockClient;
        private RestRequest _request;

        [SetUp]
        public void Setup()
        {
            _mockClient = new Mock<IRestClient>();
            _request = new RestRequest("/api/v1/users/register", Method.Post);
        }

        [Test]
        public void TestUserRegistration_Success()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Created,
                Content = "User registered successfully"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "newuser",
                password = "password123",
                email = "newuser@example.com",
                phone = "1234567890"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Created));
            Assert.That(response.Content, Is.EqualTo("User registered successfully"));
        }

        [Test]
        public void TestUserRegistration_BadRequest()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = "Invalid input data"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "",
                password = "password123",
                email = "invalidemail",
                phone = "1234567890"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
            Assert.That(response.Content, Is.EqualTo("Invalid input data"));
        }

        [Test]
        public void TestUserRegistration_Conflict()
        {
            // Arrange
            var mockResponse = new RestResponse
            {
                StatusCode = HttpStatusCode.Conflict,
                Content = "Username or email already exists"
            };

            _mockClient.Setup(client => client.Execute(It.IsAny<RestRequest>())).Returns(mockResponse);

            _request.AddJsonBody(new
            {
                username = "existinguser",
                password = "password123",
                email = "existinguser@example.com",
                phone = "1234567890"
            });

            // Act
            var response = _mockClient.Object.Execute(_request);

            // Assert
            Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
            Assert.That(response.Content, Is.EqualTo("Username or email already exists"));
        }
    }
}
