using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToggleApi.Models;

namespace ToggleApi.Queries
{
    public class FetchTogglesForClient : IQuery<IEnumerable<Toggle>>
    {
        public string ClientId { get; set; }
        public string ClientVersion { get; set; }
    }
}
