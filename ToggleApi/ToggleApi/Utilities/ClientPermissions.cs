using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Utilities
{

    public class ClientPermissions 
    {
        public ClientPermissions(IEnumerable<Client> whitelist, IEnumerable<Client> customValues)
        {
            Whitelist = whitelist;
            CustomValues = customValues;
        }

        public IEnumerable<Client> Whitelist { get; set; }

        public IEnumerable<Client> CustomValues { get; set; }
    }


}
