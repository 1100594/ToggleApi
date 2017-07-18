namespace ToggleApi.Commands
{
    public interface ICommandHandler
		: ICommandHandler<CreateToggle>
        , ICommandHandler<UpdateToggleValue>
        , ICommandHandler<DeleteToggle>
        , ICommandHandler<AddToWhitelist>
        , ICommandHandler<UpdateWhitelist>
        , ICommandHandler<AddToCustomValues>
        , ICommandHandler<UpdateToggleCustomValue>
        , ICommandHandler<DeleteClientToggle>
    {
        
    }
    public interface ICommandHandler<in TCommand> where TCommand : ICommand
    {
        void Execute(TCommand command);
    }
}
