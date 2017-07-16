using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToggleApi.Commands
{
    public class UpdateWhitelist : ICommand
    {
        public string ToggleName { get; }
        public string ClientId { get; }
        public string ClientVersion { get; }

        public UpdateWhitelist(string toggleName, string clientId, string clientVersion)
        {
            ToggleName = toggleName;
            ClientId = clientId;
            ClientVersion = clientVersion;
        }
    }
}
