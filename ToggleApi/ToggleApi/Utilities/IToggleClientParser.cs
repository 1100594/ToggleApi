namespace ToggleApi.Utilities
{
    public interface IToggleClientParser
    {
        string Input { get; set; }
        ClientPermissions Extract();
        bool IsValid();
    }
}