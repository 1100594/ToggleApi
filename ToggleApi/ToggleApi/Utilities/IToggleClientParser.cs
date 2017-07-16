namespace ToggleApi.Utilities
{
    public interface IToggleClientParser
    {
        string Input { get; set; }
        IClientPermissions Extract();
        bool IsValid();
    }
}