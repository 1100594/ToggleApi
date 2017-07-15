using System;

namespace ToggleApi.Commands
{
    public interface ICommand
    {
        Guid Id { get; }
    }
}
