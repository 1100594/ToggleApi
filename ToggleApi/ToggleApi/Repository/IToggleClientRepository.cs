using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public interface IToggleClientRepository
    {
        IEnumerable<KeyValuePair<string, bool>> GetTogglesForClient(string clientId, string clientVersion);
        Toggle GetToggleByName(string toggleName);
        void Save(Toggle toggle);
        void AddToWhiteList(string toggleName, IEnumerable<Client> whitelist);
        void AddToCustomValues(string toggleName, IEnumerable<Client> customValues);
        void UpdateToggleValue(string toggleName, bool toggleValue);
        void UpdateToggleWhitelist(string toggleName, string clientId, string clientVersion);
        void UpdateToggleCustomValue(string toggleName, bool toggleValue, string clientId, string clientVersion);
        void Delete(string toggleName);
        void DeleteClient(string toggleName, string clientId, string clientVersion);
    }
}
