using NUnit.Framework;
using Moq;
using MyCloa.Common;
using MyCloa.Common.Api;
using MyCloa.Common.Command;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;

[TestFixture]
public class ApiHelperTests
{
    private ApiHelper _apiHelper;
    private Mock<IResolve> _resolveMock;
    private Mock<IRemoteCommand> _remoteCommandMock;
    private Mock<IAuthentication> _authenticationMock;
    private Mock<IAuthorization> _authorizationMock;
    private Mock<ICommandHelper> _commandHelper;
    private Mock<IResolveCommandService> _resolveCommandService;

    [SetUp]
    public void Setup()
    {
        _resolveCommandService = new Mock<IResolveCommandService>();
        _resolveMock = new Mock<IResolve>();
        _remoteCommandMock = new Mock<IRemoteCommand>();
        _authenticationMock = new Mock<IAuthentication>();
        _authorizationMock = new Mock<IAuthorization>();
        _commandHelper = new Mock<ICommandHelper>();
        _apiHelper = new ApiHelper(_resolveMock.Object, _commandHelper.Object);
        
        _resolveMock.Setup(r => r.Resolve<IDataSerializer>()).Returns(new DefaultSerializer());
        ResolveCommandService.ClearResolveCommandService();
        ResolveCommandService.SetResolveCommandService(_resolveCommandService.Object);
    }

    [Test]
    public async Task Call_WithLocalCommand_ReturnsSerializedResult()
    {
        // Arrange
        var requestStringEntity = new RequestStringEntity
        {
            CommandName = "LocalCommand",
            RequestId = "123"
        };

        var commandData = new LocalCommandData();
        var expectedResult = ResultEntity.Success("123", "Result");
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.Run(requestStringEntity)).Returns("Result");
        commandMock.SetupSet(c => c.Resolve = _resolveMock.Object);

        _resolveMock.Setup(r => r.Resolve<IAuthentication>()).Returns(_authenticationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IAuthorization>()).Returns(_authorizationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);
        _resolveMock.Setup(r => r.Resolve<ICommand>()).Returns(commandMock.Object);

        
        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(true);
        _resolveCommandService.Setup(r => r.Resolve(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(commandMock.Object);
        
        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthentication(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(false);
        _commandHelper.Setup(r => r.ValidAuthorization(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(true);

        
        // Act
        var result = await _apiHelper.Call<LocalCommandData>(requestStringEntity);

        // Assert
        Assert.AreEqual("{\"Data\":\"Result\",\"Result\":true,\"ErrorMessage\":\"\",\"ErrorCode\":\"\",\"RequestId\":\"123\"}", result);
        commandMock.Verify(c => c.Run(requestStringEntity), Times.Once);
    }

    [Test]
    public async Task Call_WithRemoteCommand_CallsRemoteCommandAndReturnsResult()
    {
        // Arrange
        var requestStringEntity = new RequestStringEntity
        {
            CommandName = "RemoteCommand",
            RequestId = "123",
            ServiceName = "remoteservice",
            HttpsSchema = false
        };

        var expectedResult = "RemoteResult";

        _resolveMock.Setup(r => r.Resolve<IAuthentication>()).Returns(_authenticationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IAuthorization>()).Returns(_authorizationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);

        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "RemoteCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthentication(It.IsIn<string>(new List<string>() { "RemoteCommand" }))).Returns(false);
        _commandHelper.Setup(r => r.ValidAuthorization(It.IsIn<string>(new List<string>() { "RemoteCommand" }))).Returns(true);

        
        _remoteCommandMock.Setup(rc => rc.Execute(requestStringEntity)).ReturnsAsync(expectedResult);

        // Act
        var result = await _apiHelper.Call<CommandData>(requestStringEntity);

        // Assert
        Assert.AreEqual(expectedResult, result);
        _remoteCommandMock.Verify(rc => rc.Execute(requestStringEntity), Times.Once);
    }

    [Test]
    public async Task Call_WithInvalidCommandName_ReturnsErrorResult()
    {
        // Arrange
        var requestStringEntity = new RequestStringEntity
        {
            CommandName = "InvalidCommand",
            RequestId = ""
        };

       
        _resolveMock.Setup(r => r.Resolve<IAuthentication>()).Returns(_authenticationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IAuthorization>()).Returns(_authorizationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);
        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "LocalCommand" }))).Returns(false);
        // Act
        var result = await _apiHelper.Call<CommandData>(requestStringEntity);

        // Assert
        Assert.IsTrue(result.Contains("接口不存在。"));
    }

    [Test]
    public async Task Call_WithAuthenticationRequiredAndNotAuthenticated_ReturnsErrorResult()
    {
        // Arrange
        var requestStringEntity = new RequestStringEntity
        {
            CommandName = "AuthenticatedCommand",
            RequestId = "123"
        };

        _resolveMock.Setup(r => r.Resolve<IAuthentication>()).Returns(_authenticationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IAuthorization>()).Returns(_authorizationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);

        _authenticationMock.Setup(a => a.IsAuthentication(requestStringEntity)).Returns(true);
        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "AuthenticatedCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthentication(It.IsIn<string>(new List<string>() { "AuthenticatedCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthorization(It.IsIn<string>(new List<string>() { "AuthenticatedCommand" }))).Returns(true);

        // Act
        var result = await _apiHelper.Call<AuthenticatedCommandData>(requestStringEntity);

        // Assert
        //Assert.AreEqual("你没有权限访问此功能。", result);
        Assert.IsTrue(result.Contains("你没有权限访问此功能。"));
    }

    [Test]
    public async Task Call_WithAuthenticationAndAuthorizationRequiredAndNotAuthorized_ReturnsErrorResult()
    {
        // Arrange
        var requestStringEntity = new RequestStringEntity
        {
            CommandName = "AuthorizedCommand",
            RequestId = "123"
        };

        
        var commandMock = new Mock<ICommand>();
        commandMock.Setup(c => c.Run(requestStringEntity)).Returns("Result");
        commandMock.SetupSet(c => c.Resolve = _resolveMock.Object);
        
        var commandData = new AuthorizedCommandData(){RequestAuthentication = true, RequestAuthorization = true};

        _resolveMock.Setup(r => r.Resolve<IAuthentication>()).Returns(_authenticationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IAuthorization>()).Returns(_authorizationMock.Object);
        _resolveMock.Setup(r => r.Resolve<IRemoteCommand>()).Returns(_remoteCommandMock.Object);

        _authenticationMock.Setup(a => a.IsAuthentication(requestStringEntity)).Returns(false);
        _authorizationMock.Setup(auth => auth.IsAuthorization(requestStringEntity, commandData)).Returns(false);

        _commandHelper.Setup(r => r.CommandValid(It.IsIn<string>(new List<string>() { "AuthorizedCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthentication(It.IsIn<string>(new List<string>() { "AuthorizedCommand" }))).Returns(true);
        _commandHelper.Setup(r => r.ValidAuthorization(It.IsIn<string>(new List<string>() { "AuthorizedCommand" }))).Returns(false);
        
        _resolveCommandService.Setup(r => r.Resolve(It.IsIn<string>(new List<string>() { "AuthorizedCommand" }))).Returns(commandMock.Object);
        // Act
        var result = await _apiHelper.Call<AuthorizedCommandData>(requestStringEntity);

        // Assert
        Assert.IsTrue(result.Contains("你没有权限访问此功能。"));
    }
}

public class LocalCommandData : CommandData
{
}

public class AuthenticatedCommandData : CommandData
{
}

public class AuthorizedCommandData : CommandData
{
}