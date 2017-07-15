namespace ToggleApi.Commands
{
    public interface ICommandHandler: ICommandHandler<CreateToggle>
        , ICommandHandler<UpdateToggleValue>
        , ICommandHandler<AddToWhitelist>
        , ICommandHandler<RemoveFromWhitelist>
        , ICommandHandler<AddToCustomValues>
        , ICommandHandler<RemoveFromCustomValues>
        , ICommandHandler<DeleteToggle>
    {
        
    }
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
