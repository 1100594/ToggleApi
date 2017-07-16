using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Commands
{
    public class AddToCustomValues : ICommand
    {
        public string ToggleName { get; }
        public IEnumerable<Client> CustomValues { get; }

        public AddToCustomValues(string toggleName, IEnumerable<Client> customValues)
        {
            ToggleName = toggleName;
            CustomValues = customValues;
        }
    }
}