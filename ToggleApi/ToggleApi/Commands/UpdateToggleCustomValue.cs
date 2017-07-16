namespace ToggleApi.Commands
{
    public class UpdateToggleCustomValue : ICommand
    {
        public string ToggleName { get; }
        public bool ToggleValue { get; }
        public string ClientId { get; }
        public string ClientVersion { get; }

        public UpdateToggleCustomValue(string toggleName, bool toggleValue, string clientId, string clientVersion)
        {
            ToggleName = toggleName;
            ToggleValue = toggleValue;
            ClientId = clientId;
            ClientVersion = clientVersion;
        }
    }
}
