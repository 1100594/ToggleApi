using System.Collections.Generic;
using System.Linq;
using ToggleApi.Models;
using static ToggleApi.Utilities.Utils;

namespace ToggleApi.Repository
{
    public class ToggleClientRepository : IToggleClientRepository
    {
        private static readonly ICollection<Toggle> Toggles = new List<Toggle>();


        public IEnumerable<KeyValuePair<string, bool>> GetTogglesForClient(string clientId, string clientVersion)
        {
            var client = new Client(clientId, clientVersion);

            foreach (var toggle in Toggles)
            {
                if (toggle.IsApplicableTo(client))
                {
                    yield return new KeyValuePair<string, bool>(toggle.Name, toggle.DefaultValue);
                }
            }
        }

        public void Save(Toggle toggle)
        {
            if (!Toggles.Contains(toggle))
                Toggles.Add(toggle);
            //Handle error 
        }
        public void Delete(string toggleName)
        {
            var toggleToDelete = GetToggleByName(toggleName);
            if (toggleToDelete != null)
            {
                Toggles.Remove(toggleToDelete);
            }
            //Handle error
        }

        public void AddToWhiteList(string toggleName, ICollection<Client> whitelist)
        {
            var toggle = GetToggleByName(toggleName);
            toggle?.AddToWhitelist(whitelist);
            //Handle errors
        }

        public void AddToCustomValues(string toggleName, IDictionary<Client, bool> customValues)
        {
            var toggle = GetToggleByName(toggleName);
            toggle?.OverrideWith(customValues);
            //Handle errors
        }

        private Toggle GetToggleByName(string toggleName)
        {
            ThrowOnNullArgument(toggleName, toggleName);
            return Toggles.FirstOrDefault(t => t.Name.Equals(toggleName));
        }
    }
}
