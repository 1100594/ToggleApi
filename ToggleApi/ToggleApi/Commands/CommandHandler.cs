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
       
        public void Execute(CreateToggle createCmd)
        {
            ThrowOnNullArgument(createCmd, nameof(createCmd));

            var toggle = new Toggle(createCmd.ToggleName, createCmd.ToggleValue);
            _repository.Save(toggle);
        }

        public void Execute(UpdateToggleValue updateCmd)
        {
            ThrowOnNullArgument(updateCmd, nameof(updateCmd));
            _repository.UpdateToggleValue(updateCmd.ToggleName, updateCmd.ToggleValue);
        }

        public void Execute(AddToWhitelist addWhitelistCmd)
        {
            ThrowOnNullArgument(addWhitelistCmd, nameof(addWhitelistCmd));
            _repository.AddToWhiteList(addWhitelistCmd.ToggleName, addWhitelistCmd.Whitelist);
        }
        public void Execute(AddToCustomValues customValuesCmd)
        {
            ThrowOnNullArgument(customValuesCmd, nameof(customValuesCmd));
            _repository.AddToCustomValues(customValuesCmd.ToggleName, customValuesCmd.CustomValues);
        }


        public void Execute(DeleteToggle deleteToggleCmd)
        {
            ThrowOnNullArgument(deleteToggleCmd, nameof(deleteToggleCmd));

            _repository.Delete(deleteToggleCmd.ToggleName);
        }

        public void Execute(DeleteClientToggle deleteClientToggleCmd)
        {
            ThrowOnNullArgument(deleteClientToggleCmd, nameof(deleteClientToggleCmd));
            _repository.DeleteClient(deleteClientToggleCmd.ToggleName, deleteClientToggleCmd.ClientId, deleteClientToggleCmd.ClientVersion);
        }

        public void Execute(UpdateWhitelist updateWhitelistCmd)
        {
            ThrowOnNullArgument(updateWhitelistCmd, nameof(updateWhitelistCmd));
            _repository.UpdateToggleWhitelist(updateWhitelistCmd.ToggleName, updateWhitelistCmd.ClientId, updateWhitelistCmd.ClientVersion);
        }

        public void Execute(UpdateToggleCustomValue updateToggleCustomValueCmd)
        {
            ThrowOnNullArgument(updateToggleCustomValueCmd, nameof(updateToggleCustomValueCmd));
            _repository.UpdateToggleCustomValue(updateToggleCustomValueCmd.ToggleName, updateToggleCustomValueCmd.ToggleValue, 
                updateToggleCustomValueCmd.ClientId, updateToggleCustomValueCmd.ClientVersion);
        }
    }
}
