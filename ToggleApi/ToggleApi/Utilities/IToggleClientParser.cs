using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Utilities
{
    public interface IToggleClientParser
    {
        bool ToggleValue { get; set; }
        string Input { get; set; }
        void Extract(out ICollection<Client> whitelist, out IDictionary<Client, bool> customValues);
        bool IsValid();
    }
}