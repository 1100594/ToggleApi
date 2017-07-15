using System;
using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public class ToggleClientRepository : IToggleClientRepository
    {
        private static ICollection<Toggle> toggles = new List<Toggle>();

        public void Delete(string toggleName)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<Toggle> GetTogglesForClient()
        {
            throw new NotImplementedException();
        }

        public Toggle GetToggleByName(string toggleName)
        {
            throw new NotImplementedException();
        }

        public void Save(Toggle toggle)
        {
            toggles.Add(toggle);
        }
    }
}
