using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Commands
{
    public class AddToWhitelist : ICommand
    {
        public string ToggleName { get; }
        public IEnumerable<Client> Whitelist { get; }

        public AddToWhitelist(string toggleName, IEnumerable<Client> whitelist)
        {
            ToggleName = toggleName;
            Whitelist = whitelist;
        }
    }
}