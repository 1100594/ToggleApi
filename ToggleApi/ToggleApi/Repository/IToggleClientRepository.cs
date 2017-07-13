using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToggleApi.Models;

namespace ToggleApi.Repository
{
    public interface IToggleClientRepository
    {
        Toggle GetToggleByName(string toggleName);
        void Delete(string toggleName);
        void Save(Toggle toggle);
    }
}
