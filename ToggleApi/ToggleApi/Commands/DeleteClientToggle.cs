using ToggleApi.Commands;

public class DeleteClientToggle : ICommand
{
    public string ToggleName { get; }
    public string ClientId { get; }
    public string ClientVersion { get; }


    public DeleteClientToggle(string toggleName, string clientId, string clientVersion)
    {
        ToggleName = toggleName;
        ClientId = clientId;
        ClientVersion = clientVersion;
    }
}
