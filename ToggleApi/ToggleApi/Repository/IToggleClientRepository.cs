using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public interface IToggleClientRepository
    {
        IReadOnlyCollection<Toggle> GetTogglesForClient();
        Toggle GetToggleByName(string toggleName);
        void Delete(string toggleName);
        void Save(Toggle toggle);
    }
}
