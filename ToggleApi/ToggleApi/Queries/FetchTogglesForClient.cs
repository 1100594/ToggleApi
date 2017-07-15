using System.Collections.Generic;

namespace ToggleApi.Queries
{
    public class FetchTogglesForClient : IQuery<IEnumerable<KeyValuePair<string, bool>>>
    {
        public string ClientId { get; }
        public string ClientVersion { get; }

        public FetchTogglesForClient(string clientId, string clientVersion)
        {
            ClientId = clientId;
            ClientVersion = clientVersion;
        }
    }
}
