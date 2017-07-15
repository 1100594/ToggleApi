using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Utilities
{

    public class ClientPermissions : IClientPermissions
    {
        public ClientPermissions(ICollection<Client> whitelist, ICollection<Client> customValues)
        {
            Whitelist = whitelist;
            CustomValues = customValues;
        }

        public ICollection<Client> Whitelist { get; set; }

        public ICollection<Client> CustomValues { get; set; }
    }


}
