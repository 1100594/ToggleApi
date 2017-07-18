using Moq;
using ToggleApi.Commands;
using ToggleApi.Models;
using ToggleApi.Repository;
using Xunit;

namespace ToggleApiTest.Commands
{
    public class CommandHandlerTest
    {
        private readonly ICommandHandler _handler;
        private readonly Mock<IToggleClientRepository> _toggleClientRepoMock;

        public CommandHandlerTest()
        {
            _toggleClientRepoMock = new Mock<IToggleClientRepository>();

            _handler = new CommandHandler(_toggleClientRepoMock.Object);
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void CreateToggle()
        {
            const string toggleName = "toggleA";
            const bool toggleValue = true;

            _toggleClientRepoMock
                .Setup(repo => repo.Save(new Toggle(toggleName, toggleValue)))
                .Verifiable();

            _handler.Execute(new CreateToggle(toggleName, toggleValue));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void UpdateToggleValue()
        {
            const string toggleName = "toggleA";
            const bool toggleValue = false;

            _toggleClientRepoMock
                .Setup(repo => repo.UpdateToggleValue(toggleName, toggleValue))
                .Verifiable();

            _handler.Execute(new UpdateToggleValue(toggleName, toggleValue));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void DeleteToggleValue()
        {
            const string toggleName = "toggleA";

            _toggleClientRepoMock
                .Setup(repo => repo.Delete(toggleName))
                .Verifiable();

            _handler.Execute(new DeleteToggle(toggleName));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void AddWhitelist()
        {
            const string toggleName = "toggle1";
            var toggleWhitelist = new[]
            {
                new Client("id1", "*")
            };

            _toggleClientRepoMock
                .Setup(repo => repo.AddToWhiteList(toggleName, toggleWhitelist))
                .Verifiable();

            _handler.Execute(new AddToWhitelist(toggleName, toggleWhitelist));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void UpdateWhitelist()
        {
            const string toggleName = "toggle2";
            const string clientId = "id1";
            const string clientVersion = "*";

            _toggleClientRepoMock
                .Setup(repo => repo.UpdateToggleWhitelist(toggleName, clientId, clientVersion))
                .Verifiable();

            _handler.Execute(new UpdateWhitelist(toggleName, clientId, clientVersion));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void AddToCustomValue()
        {
            const string toggleName = "toggle3";
            var toggleCustomValue = new[]
            {
                new Client("id1", "*")
            };

            _toggleClientRepoMock
                .Setup(repo => repo.AddToWhiteList(toggleName, toggleCustomValue));

            _handler.Execute(new AddToCustomValues(toggleName, toggleCustomValue));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void UpdateCustomToggleValue()
        {
            const string toggleName = "toggleA";
            const bool toggleValue = false;
            const string clientId = "client1";
            const string clientVersion = "*";

            _toggleClientRepoMock
                .Setup(repo => repo.UpdateToggleCustomValue(toggleName, toggleValue, 
                clientId, clientVersion));

            _handler.Execute(new UpdateToggleCustomValue(toggleName, toggleValue, clientId, clientVersion));

            _toggleClientRepoMock.Verify();
        }

        [Fact]
        [Trait("UnitTest", "CommandHandler")]
        public void DeleteClientFromToggle()
        {
            const string toggleName = "toggleA";
            const string clientId = "client1";
            const string clientVersion = "*";

            _toggleClientRepoMock
                .Setup(repo => repo.DeleteClient(toggleName,clientId,clientVersion));

            _handler.Execute(new DeleteClientToggle(toggleName, clientId, clientVersion));

            _toggleClientRepoMock.Verify();
        }
    }
}
