using System;
using ToggleApi.Models;
using ToggleApi.Repository;
using ToggleApi.Utilities;

namespace ToggleApi.Commands
{
    public class CommandHandler : 
          ICommandHandler<CreateToggle>
        , ICommandHandler<UpdateToggleValue>
        , ICommandHandler<AddToWhitelist>
        , ICommandHandler<RemoveFromWhitelist>
        , ICommandHandler<AddToCustomValues>
        , ICommandHandler<RemoveFromCustomValues>
        , ICommandHandler<DeleteToggle>
    {
        #region Private Variables
        private readonly IToggleClientRepository _repository;
        #endregion

        #region Construtors
        public CommandHandler(IToggleClientRepository toogleClientRepository)
        {
            Utils.ThrowOnNullArgument(toogleClientRepository, nameof(toogleClientRepository));
            _repository = toogleClientRepository;
        }
        #endregion

        #region Public Methods
        public void Execute(CreateToggle createCommand)
        {
            Utils.ThrowOnNullArgument(createCommand, nameof(createCommand));

            var toggle = new Toggle(createCommand.ToggleName, createCommand.ToggleValue);
            _repository.Save(toggle);
        }

        public void Execute(UpdateToggleValue command)
        {
            throw new NotImplementedException();
        }

        public void Execute(AddToWhitelist command)
        {
            throw new NotImplementedException();
        }

        public void Execute(RemoveFromWhitelist command)
        {
            throw new NotImplementedException();
        }

        public void Execute(AddToCustomValues command)
        {
            throw new NotImplementedException();
        }

        public void Execute(RemoveFromCustomValues command)
        {
            throw new NotImplementedException();
        }

        public void Execute(DeleteToggle deleteToggleCommand)
        {
            Utils.ThrowOnNullArgument(deleteToggleCommand, nameof(deleteToggleCommand));

            _repository.Delete(deleteToggleCommand.ToggleName);
        }
        #endregion
    }
}
