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
                    var toggleValue = toggle.GetValueFor(client);
                    var toggleName = toggle.Name;
                    yield return new KeyValuePair<string, bool>(toggleName, toggleValue);
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
            toggle.AddToCustomValues(customValues);
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

        public void UpdateToggleValue(string toggleName, bool toggleValue)
        {
            var toggleToUpdate = GetToggleByName(toggleName);
            if (toggleToUpdate.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} toggle does not exist");
            }

            var defaultValue = toggleToUpdate.DefaultValue;
            if (defaultValue == toggleValue)
            {
                throw new NotSupportedException($"The default value of toggle {toggleName} already is {toggleValue}");
            }
            toggleToUpdate.DefaultValue = toggleValue;
        }

        public void DeleteClient(string toggleName, string clientId, string clientVersion)
        {
            var toggleToUpdate = GetToggleByName(toggleName);
            if (toggleToUpdate.IsNull())
            {
                throw new ArgumentException($"The requested {toggleName} toggle does not exists");
            }
            //TODO Review this 
            toggleToUpdate.DettachFrom(new Client(clientId, clientVersion));
        }

        public Toggle GetToggleByName(string toggleName)
        {
            ThrowOnNullArgument(toggleName);
            return Toggles.FirstOrDefault(t => t.Name.Equals(toggleName));
        }
    }
}
