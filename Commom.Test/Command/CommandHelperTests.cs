using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using MyCloa.Common.Ioc;

namespace MyCloa.Common.Command
{
    [TestFixture]
    public class CommandHelperTests
    {
        private static ICommandHelper _commandHelper = DefaultCommandHelper.Instance;
        static CommandHelperTests()
        {
            _commandHelper.ScanCommand<CommandAttribute, CommandData>(new Assembly[]{Assembly.GetExecutingAssembly()});
        }
        [Test]
        public void ScanCommand_Exception()
        {
            Assert.Catch<BusinessException>(() =>
            {
                _commandHelper.ScanCommand<CommandAttribute, CommandData>(new Assembly[]
                    { Assembly.GetExecutingAssembly() });
            });
        }
        [Test]
        public void CommandValid_ExistingCommand_ReturnsTrue()
        {
            // Arrange
            
            string commandName = "ExistingCommand";

            // Act
            bool result = _commandHelper.CommandValid(commandName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void CommandValid_NonExistingCommand_ReturnsFalse()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "NonExistingCommand";

            // Act
            bool result = _commandHelper.CommandValid(commandName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidAuthentication_CommandRequiresAuthentication_ReturnsTrue()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "ExistingCommand";

            // Act
            bool result = _commandHelper.ValidAuthentication(commandName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidAuthentication_CommandDoesNotRequireAuthentication_ReturnsFalse()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "CommandDoesNotRequireAuthentication";

            // Act
            bool result = _commandHelper.ValidAuthentication(commandName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void ValidAuthorization_CommandRequiresAuthorization_ReturnsTrue()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "ExistingCommand";

            // Act
            bool result = _commandHelper.ValidAuthorization(commandName);

            // Assert
            Assert.IsTrue(result);
        }

        [Test]
        public void ValidAuthorization_CommandDoesNotRequireAuthorization_ReturnsFalse()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "CommandDoesNotRequireAuthorization";

            // Act
            bool result = _commandHelper.ValidAuthorization(commandName);

            // Assert
            Assert.IsFalse(result);
        }

        [Test]
        public void GetCommandData_ExistingCommand_ReturnsCommandData()
        {
            // Arrange
           // CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "ExistingCommand";

            // Act
            CommandData result = _commandHelper.GetCommandData<CommandData>(commandName);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(typeof(ExistingCommand), result.CommandType);
            Assert.IsTrue(result.RequestAuthentication);
            Assert.IsTrue(result.RequestAuthorization);
        }

        [Test]
        public void GetCommandData_NonExistingCommand_ReturnsNull()
        {
            // Arrange
            //CommandHelper.ScanCommand<CommandAttribute, CommandData>(Assembly.GetExecutingAssembly());
            string commandName = "NonExistingCommand";

            // Act
            CommandData result = _commandHelper.GetCommandData<CommandData>(commandName);

            // Assert
            Assert.IsNull(result);
        }

        [Command(Name ="ExistingCommand", RequestAuthentication = true, RequestAuthorization = true)]
        private class ExistingCommand : ICommand
        {
            public IResolve Resolve { get; set; }
            public string Run(RequestStringEntity requestStringEntity)
            {
                throw new NotImplementedException();
            }
        }
    }
}