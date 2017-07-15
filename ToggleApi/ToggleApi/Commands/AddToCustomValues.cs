using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Commands
{
    public class AddToCustomValues : BaseCommand
    {
        public string ToggleName { get; }
        public IDictionary<Client, bool> CustomValues { get; }

        public AddToCustomValues(string toggleName, IDictionary<Client, bool> customValues)
        {
            ToggleName = toggleName;
            CustomValues = customValues;
        }
    }
}