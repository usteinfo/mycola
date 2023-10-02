namespace MyCloa.Common.Ioc;

[TestFixture]
public class ResolveBaseTests
{
    private class TestCommand : ICommand
    {
        public IResolve Resolve { get; set; }

        public string Run(RequestStringEntity requestStringEntity)
        {
            throw new NotImplementedException();
        }
    }

    private class TestResolveBase : ResolveBase
    {
        public TestResolveBase(IResolveCommandService resolveCommandService) : base(resolveCommandService)
        {
        }

        public override T Resolve<T>()
        {
            return default(T);
        }
    }

    [Test]
    public void Resolve_CommandName_ValidName_ReturnsCommand()
    {
        // Arrange
        var resolveBase = new TestResolveBase(new TestResolveCommandService());

        // Act
        var result = resolveBase.Resolve("TestCommand");

        // Assert
        Assert.IsNotNull(result);
        Assert.IsInstanceOf<TestCommand>(result);
        Assert.AreEqual(resolveBase, result.Resolve);
    }

    [Test]
    public void Resolve_CommandName_InvalidName_ThrowsException()
    {
        // Arrange
        var resolveBase = new TestResolveBase(new TestResolveCommandService());

        // Act & Assert
        Assert.Throws<BusinessException>(() => resolveBase.Resolve("InvalidCommand"));
    }

    private class TestResolveCommandService : IResolveCommandService
    {
        public void Register(string name, Type targetType)
        {
            throw new NotImplementedException();
        }

        public ICommand Resolve(string name)
        {
            if (name == "TestCommand")
            {
                return new TestCommand();
            }

            return null;
        }
    }
}