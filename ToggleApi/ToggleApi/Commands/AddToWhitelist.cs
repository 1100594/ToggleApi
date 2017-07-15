using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Commands
{
    public class AddToWhitelist : BaseCommand
    {
        public string ToggleName { get; }
        public ICollection<Client> Whitelist { get; }

        public AddToWhitelist(string toggleName, ICollection<Client> whitelist)
        {
            ToggleName = toggleName;
            Whitelist = whitelist;
        }
    }
}