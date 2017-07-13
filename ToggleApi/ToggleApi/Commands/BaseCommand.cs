using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToggleApi.Commands
{
    public class BaseCommand: ICommand
    {
        public BaseCommand()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
    }
}
