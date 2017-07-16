namespace ToggleApi.Commands
{
    public class UpdateToggleValue : ICommand
    {
        public string ToggleName { get; }
        public bool ToggleValue { get; }

        public UpdateToggleValue(string toggleName, bool toggleValue)
        {
            ToggleName = toggleName;
            ToggleValue = toggleValue;
        }
    }
}