using System;
using ToggleApi.Models;
using ToggleApi.Repository;

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
        private IToggleClientRepository repository;
        #endregion

        #region Construtors
        public CommandHandler(IToggleClientRepository repository)
        {
            this.repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        #endregion

        #region Private & Internal Methods
        private static void ThrowOnNullArgument(ICommand command)
        {
            if (command == null)
                throw new ArgumentNullException(nameof(command));
        }
        #endregion

        #region Public Methods
        public void Execute(CreateToggle command)
        {
            ThrowOnNullArgument(command);

            var toggle = new Toggle(command.ToggleName, command.ToggleValue);
            repository.Save(toggle);
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

        public void Execute(DeleteToggle command)
        {
            ThrowOnNullArgument(command);

            repository.Delete(command.ToggleName);
        }
        #endregion
    }
}
