namespace ToggleApi.Commands
{
    public class DeleteToggle : ICommand
    {
        public DeleteToggle(string toggleName)
        {
            ToggleName = toggleName;
        }

        public string ToggleName { get; }
    }
}
