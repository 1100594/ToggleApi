using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToggleApi.Commands
{
    public class DeleteToggle : BaseCommand
    {
        private string toggleName;

        public DeleteToggle(string toggleName)
        {
            ToggleName = toggleName;
        }

        public string ToggleName { get => toggleName; set => toggleName = value; }
    }
}
