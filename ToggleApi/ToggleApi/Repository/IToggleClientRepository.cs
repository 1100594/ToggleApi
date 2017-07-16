using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public interface IToggleClientRepository
    {
        IEnumerable<KeyValuePair<string, bool>> GetTogglesForClient(string clientId, string clientVersion);
        Toggle GetToggleByName(string toggleName);
        void Save(Toggle toggle);
        void Delete(string toggleName);
        void AddToWhiteList(string toggleName, ICollection<Client> whitelist);
        void AddToCustomValues(string toggleName, ICollection<Client> customValues);
        void UpdateToggleValue(string toggleName, bool toggleValue);
    }
}
