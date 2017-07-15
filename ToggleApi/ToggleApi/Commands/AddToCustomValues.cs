using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Commands
{
    public class AddToCustomValues : ICommand
    {
        public string ToggleName { get; }
        public ICollection<Client> CustomValues { get; }

        public AddToCustomValues(string toggleName, ICollection<Client> customValues)
        {
            ToggleName = toggleName;
            CustomValues = customValues;
        }
    }
}