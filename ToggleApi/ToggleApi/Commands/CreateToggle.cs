namespace ToggleApi.Commands
{
    public class CreateToggle : ICommand
    {
        public CreateToggle(string toggleName, bool toggleValue)
        {
            ToggleName = toggleName;
            ToggleValue = toggleValue;
        }

        public string ToggleName { get;}

        public bool ToggleValue { get; }
    }
}
