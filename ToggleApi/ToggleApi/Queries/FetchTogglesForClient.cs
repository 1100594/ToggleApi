using System.Collections.Generic;

namespace ToggleApi.Queries
{
    public class FetchTogglesForClient : IQuery<IEnumerable<KeyValuePair<string, bool>>>
    {
        public string ClientId { get; set; }
        public string ClientVersion { get; set; }
    }
}
