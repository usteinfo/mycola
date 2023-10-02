using Moq;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;
using MyCloa.Common.Valid;

namespace MyCloa.Common.Command
{
    [TestFixture]
    public class CommandBaseTests
    {
        private Mock<IResolve> _resolveMock;
        private Mock<IDataSerializer> _dataSerializerMock;
        private Mock<IValidRequest> _validRequestMock;
        private Mock<IRemoteCommand> _remoteCommandMock;

        [SetUp]
        public void Setup()
        {
            _resolveMock = new Mock<IResolve>();
            _dataSerializerMock = new Mock<IDataSerializer>();
            _validRequestMock = new Mock<IValidRequest>();
            _remoteCommandMock = new Mock<IRemoteCommand>();

            _resolveMock.Setup(r => r.Resolve<IDataSerializer>()).Returns(_dataSerializerMock.Object);
            _resolveMock.Setup(r => r.Resolve<IValidRequest>()).Returns(_validRequestMock.Object);
            _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);
        }

        [Test]
        public void Run_ValidRequestStringEntity_ReturnsSerializedData()
        {
            // Arrange
            var command = new TestCommand();
            ((ICommand)command).Resolve = _resolveMock.Object;
            var requestStringEntity = new RequestStringEntity
            {
                Data = "{\"name\":\"John\",\"age\":30}"
            };
            var requestEntity =  new TestRequest(){Name = "John",Age = 30};

            _dataSerializerMock.Setup(s => s.Deserialize<TestRequest>(It.IsAny<string>())).Returns(requestEntity);
            
            var expectedResult = "serialized data";

            _dataSerializerMock.Setup(s => s.Serializer(It.IsAny<object>())).Returns(expectedResult);
            _resolveMock.Setup(r => r.Resolve<ICommand>()).Returns(command);
            _validRequestMock.Setup(r=>r.Valid(It.IsAny<TestRequest>())).Returns(new ValidResult(true,""));
            // Act
            var result = command.Run(requestStringEntity);

            // Assert
            Assert.AreEqual(expectedResult, result);
            _dataSerializerMock.Verify(s => s.Serializer(It.IsAny<object>()), Times.Once);
        }

        [Test]
        public void Run_EmptyRequestStringEntity_ThrowsBusinessException()
        {
            // Arrange
            var command = new TestCommand();
            var requestStringEntity = new RequestStringEntity();

            // Act and Assert
            Assert.Throws<BusinessException>(() => command.Run(requestStringEntity));
        }

        [Test]
        public void ConvertRequest_StringRequest_ReturnsStringObject()
        {
            // Arrange
            var command = new TestCommand();
            var request = "test request";

            // Act
            var result = command.ConvertRequest1<string>(request);

            // Assert
            Assert.AreEqual(request, result);
        }

        [Test]
        public void ConvertRequest_JsonRequest_ReturnsDeserializedObject()
        {
            // Arrange
            var command = new TestCommand();
            ((ICommand)command).Resolve = _resolveMock.Object;
            var request = "{\"Name\":\"John\",\"Age\":30}";
            var expectedObject = new TestRequest { Name = "John", Age = 30 };

            _dataSerializerMock.Setup(s => s.Deserialize<TestRequest>(request)).Returns(expectedObject);

            // Act
            var result = command.ConvertRequest1<TestRequest>(request);

            // Assert
            Assert.AreEqual(expectedObject, result);
            _dataSerializerMock.Verify(s => s.Deserialize<TestRequest>(request), Times.Once);
        }

        // Other test cases for the remaining functions...

        private class TestCommand : CommandBase<TestRequest, TestResult>
        {
            protected override TestResult ExecuteCore(TestRequest request)
            {
                return new TestResult();
            }

            public T ConvertRequest1<T>(string request)
            {
                return base.ConvertRequest<T>(request);
            }
        }

        private class TestRequest
        {
            public string Name { get; set; }
            public int Age { get; set; }
        }

        private class TestResult
        {
        }
    }
}