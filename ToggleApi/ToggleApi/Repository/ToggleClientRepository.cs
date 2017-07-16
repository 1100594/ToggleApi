using System;
using System.Collections.Generic;
using System.Linq;
using ToggleApi.Models;
using ToggleApi.Utilities;
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
            //TODO Review exceptions
            if (Toggles.Contains(toggle))
            {
                throw new ArgumentException($"The requested {toggle.Name} already exists");
            }
            Toggles.Add(toggle);
        }

        public void AddToWhiteList(string toggleName, ICollection<Client> whitelist)
        {
            var toggle = GetToggleByName(toggleName);
            if (toggle.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} client does not exists");
            }
            toggle.AddToWhitelist(whitelist);
        }

        public void AddToCustomValues(string toggleName, ICollection<Client> customValues)
        {
            var toggle = GetToggleByName(toggleName);
            if (toggle.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} client does not exists");
            }
            toggle.OverrideWith(customValues);
        }
        public void Delete(string toggleName)
        {
            var toggleToDelete = GetToggleByName(toggleName);
            if (toggleToDelete.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} client does not exists");
            }
            Toggles.Remove(toggleToDelete);
        }

        public void UpdateToggleValue(string toggleName, bool toogleValue)
        {
            var toggleToUpdate = GetToggleByName(toggleName);
            if (toggleToUpdate.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} client does not exists");
            }
            //TODO review this read only props
            Toggles.Add(new Toggle(toggleToUpdate.Name, toogleValue));
            Toggles.Remove(toggleToUpdate);
        }

        public Toggle GetToggleByName(string toggleName)
        {
            ThrowOnNullArgument(toggleName);
            return Toggles.FirstOrDefault(t => t.Name.Equals(toggleName));
        }
    }
}
