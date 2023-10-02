namespace MyCloa.Common.Ioc
{
    [TestFixture]
    public class ResolveCommandServiceTests
    {
        [Test]
        public void Register_ShouldAddCommandToDictionary()
        {
            // Arrange
            var resolveCommandService = ResolveCommandService.Instance;
            string name = "CommandName_ShouldAddCommand";
            Type targetType = typeof(CommandType);

            // Act
            resolveCommandService.Register(name, targetType);

            // Assert
            Assert.IsTrue(resolveCommandService.Resolve(name) != null);
        }

        [Test]
        public void Resolve_ShouldReturnRegisteredCommand()
        {
            // Arrange
            var resolveCommandService = ResolveCommandService.Instance;
            string name = "CommandName_ShouldReturn";
            Type targetType = typeof(CommandType);
            resolveCommandService.Register(name, targetType);

            // Act
            var command = resolveCommandService.Resolve(name);

            // Assert
            Assert.IsTrue(command != null);
            Assert.IsTrue(command.GetType() == targetType);
        }

        [Test]
        public void Resolve_ShouldThrowException_WhenCommandNameNotRegistered()
        {
            // Arrange
            var resolveCommandService = ResolveCommandService.Instance;
            string name = "NonExistingCommand";

            // Act & Assert
            Assert.Throws<BusinessException>(() => resolveCommandService.Resolve(name));
        }

        [Test]
        public void SetResolveCommandService_ShouldThrowException_WhenCalledMoreThanOnce()
        {
            // Arrange
            var resolveCommandService = ResolveCommandService.Instance;
            var newResolveCommandService = ResolveCommandService.Instance;
            ResolveCommandService.SetResolveCommandService(resolveCommandService);
            // Act & Assert
            Assert.Throws<BusinessException>(() => ResolveCommandService.SetResolveCommandService(newResolveCommandService));
        }
    }

    internal class CommandType : ICommand
    {
        public void Execute()
        {
            // Command execution logic
        }

        public IResolve Resolve { get; set; }
        public string Run(RequestStringEntity requestStringEntity)
        {
            throw new NotImplementedException();
        }
    }
}