using Moq;
using Moq.Protected;

namespace MyCloa.Common.Command
{
    [TestFixture]
    public class ExecuteTests
    {
        private HttpClient _httpClient;
        private DefaultRemoteCommand _defaultRemoteCommand;
        private Mock<HttpMessageHandler> _httpMessageHandlerMock;

        [SetUp]
        public void Setup()
        {
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _defaultRemoteCommand = new DefaultRemoteCommand(_httpClient);
        }

        [TearDown]
        public void TearDown()
        {
            _httpClient.Dispose();
        }

        [Test]
        public async Task Execute_ShouldReturnCorrectResult()
        {
            // Arrange
            var requestStringEntity = new RequestStringEntity
            {
                ServiceName = "remoteserver"
                // Set requestStringEntity properties
            };
            
            string expectedRemotePath = "/cloa/invoke";
            string expectedContent = "command content";
            HttpResponseMessage responseMessage = new HttpResponseMessage();
            responseMessage.Content = new StringContent(expectedContent);
            
            _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            // Act
            var result = await _defaultRemoteCommand.Execute(requestStringEntity);

            // Assert
            // Assert the expected result based on the input
            Assert.That(result, Is.EqualTo(expectedContent));
        }

        [Test]
        public void Execute_ShouldThrowException_WhenRequestStringEntityIsNull()
        {
            // Arrange
            RequestStringEntity requestStringEntity = null;

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await _defaultRemoteCommand.Execute(requestStringEntity));
        }

        [Test]
        public void Execute_ShouldThrowException_WhenHttpClientIsNotInitialized()
        {
            // Arrange
            var yourClass = new DefaultRemoteCommand(null);
            var requestStringEntity = new RequestStringEntity()
            {
                ServiceName = "remoteurl"
            };

            // Act and Assert
            Assert.ThrowsAsync<ArgumentNullException>(async () => await yourClass.Execute(requestStringEntity));
        }
    }
}