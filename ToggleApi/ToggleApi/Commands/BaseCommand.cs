using System;


namespace ToggleApi.Commands
{
    public class BaseCommand: ICommand
    {
        public BaseCommand()
        {                                                                                       
            Id = Guid.NewGuid();
        }

        public Guid Id { get; }
    }
}
