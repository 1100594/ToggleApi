using System;
using ToggleApi.Models;
using ToggleApi.Repository;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Commands
{
    public class CommandHandler : ICommandHandler

    {
        private readonly IToggleClientRepository _repository;

        public CommandHandler(IToggleClientRepository toggleClientRepository)
        {
            ThrowOnNullArgument(toggleClientRepository, nameof(toggleClientRepository));
            _repository = toggleClientRepository;
        }
       
        public void Execute(CreateToggle createCommand)
        {
            ThrowOnNullArgument(createCommand, nameof(createCommand));

            var toggle = new Toggle(createCommand.ToggleName, createCommand.ToggleValue);
            _repository.Save(toggle);
        }

        public void Execute(UpdateToggleValue updatecommand)
        {
            ThrowOnNullArgument(updatecommand, nameof(updatecommand));
            _repository.UpdateToggleValue(updatecommand.ToggleName, updatecommand.ToggleValue);
        }

        public void Execute(AddToWhitelist addWhitelistCommand)
        {
            ThrowOnNullArgument(addWhitelistCommand, nameof(addWhitelistCommand));
            _repository.AddToWhiteList(addWhitelistCommand.ToggleName, addWhitelistCommand.Whitelist);
        }
        public void Execute(AddToCustomValues customValuesCommand)
        {
            ThrowOnNullArgument(customValuesCommand, nameof(customValuesCommand));
            _repository.AddToCustomValues(customValuesCommand.ToggleName, customValuesCommand.CustomValues);
        }


        public void Execute(DeleteToggle deleteToggleCommand)
        {
            ThrowOnNullArgument(deleteToggleCommand, nameof(deleteToggleCommand));

            _repository.Delete(deleteToggleCommand.ToggleName);
        }

        public void Execute(DeleteClientToggle deleteClientToggleCommand)
        {
            ThrowOnNullArgument(deleteClientToggleCommand, nameof(deleteClientToggleCommand));
            _repository.DeleteClient(deleteClientToggleCommand.ToggleName, deleteClientToggleCommand.ClientId, deleteClientToggleCommand.ClientVersion);
        }
    }
}
