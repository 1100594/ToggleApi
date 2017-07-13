namespace ToggleApi.Commands
{
    public class CreateToggle : BaseCommand
    {
        private string toggleName;
        private bool toggleValue;

        public CreateToggle(string toggleName, bool toggleValue)
        {
            this.ToggleName = toggleName;
            this.ToggleValue = toggleValue;
        }

        public string ToggleName { get => toggleName; set => toggleName = value; }
        public bool ToggleValue { get => toggleValue; set => toggleValue = value; }
    }
}
