using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Utilities
{
    public interface IClientPermissions
    {
        ICollection<Client> CustomValues { get; set; }
        ICollection<Client> Whitelist { get; set; }
    }
}