using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public interface IToggleClientRepository
    {
        IEnumerable<KeyValuePair<string, bool>> GetTogglesForClient(string clientId, string clientVersion);
        void Save(Toggle toggle);
        void Delete(string toggleName);
        void AddToWhiteList(string toggleName, ICollection<Client> whitelist);
        void AddToCustomValues(string toggleName, ICollection<Client> customValues);
    }
}
