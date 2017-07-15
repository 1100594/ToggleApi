using System;
using ToggleApi.Models;
using ToggleApi.Repository;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Commands
{
    public class CommandHandler : ICommandHandler

    {
        #region Private Variables
        private readonly IToggleClientRepository _repository;
        #endregion

        #region Construtors
        public CommandHandler(IToggleClientRepository toggleClientRepository)
        {
            ThrowOnNullArgument(toggleClientRepository, nameof(toggleClientRepository));
            _repository = toggleClientRepository;
        }
        #endregion

        #region Public Methods
        public void Execute(CreateToggle createCommand)
        {
            ThrowOnNullArgument(createCommand, nameof(createCommand));

            var toggle = new Toggle(createCommand.ToggleName, createCommand.ToggleValue);
            _repository.Save(toggle);
        }

        public void Execute(UpdateToggleValue command)
        {
            throw new NotImplementedException();
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

        public void Execute(RemoveFromWhitelist command)
        {
            throw new NotImplementedException();
        }


        public void Execute(RemoveFromCustomValues command)
        {
            throw new NotImplementedException();
        }

        public void Execute(DeleteToggle deleteToggleCommand)
        {
            ThrowOnNullArgument(deleteToggleCommand, nameof(deleteToggleCommand));

            _repository.Delete(deleteToggleCommand.ToggleName);
        }
        #endregion
    }
}
