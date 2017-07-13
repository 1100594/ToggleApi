using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
