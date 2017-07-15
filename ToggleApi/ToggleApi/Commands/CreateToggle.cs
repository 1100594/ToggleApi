using System.Linq.Expressions;

namespace ToggleApi.Commands
{
    public class CreateToggle : BaseCommand
    {
        public CreateToggle(string toggleName, bool toggleValue)
        {
            ToggleName = toggleName;
            ToggleValue = toggleValue;
        }

        public string ToggleName { get; set; }

        public bool ToggleValue { get; set; }
    }
}
