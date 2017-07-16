namespace ToggleApi.Commands
{
    public interface ICommandHandler: ICommandHandler<CreateToggle>
        , ICommandHandler<UpdateToggleValue>
        , ICommandHandler<AddToWhitelist>
        , ICommandHandler<AddToCustomValues>
        , ICommandHandler<DeleteClientToggle>
        , ICommandHandler<DeleteToggle>
    {
        
    }
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
