namespace ToggleApi.Utilities
{
    public interface IToggleClientParser
    {
        bool ToggleValue { get; set; }
        string Input { get; set; }
        IClientPermissions Extract();
        bool IsValid();
    }
}