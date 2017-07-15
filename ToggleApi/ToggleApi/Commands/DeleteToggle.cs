namespace ToggleApi.Commands
{
    public class DeleteToggle : BaseCommand
    {
        public DeleteToggle(string toggleName)
        {
            ToggleName = toggleName;
        }

        public string ToggleName { get; set; }
    }
}
