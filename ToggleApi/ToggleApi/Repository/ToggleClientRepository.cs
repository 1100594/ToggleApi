using System;
using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public class ToggleClientRepository : IToggleClientRepository
    {
        private static readonly ICollection<Toggle> Toggles = new List<Toggle>();

        public void Delete(string toggleName)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Toggle> GetTogglesForClient(string clientId, string clientVersion)
        {
            var client = new Client(clientId, clientVersion);

            foreach (var toggle in Toggles)
            {
                if (toggle.IsApplicableTo(client))
                {
                    //Returns a IEnumerable<Toogle>
                    yield return toggle;
                }
            }
        }

        public void Save(Toggle toggle)
        {
            if (!Toggles.Contains(toggle))
                Toggles.Add(toggle);
        }
    }
}
