using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToggleApi.Commands
{
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
