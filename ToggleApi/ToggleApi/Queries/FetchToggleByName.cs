using ToggleApi.Models;

namespace ToggleApi.Queries
{
    public class FetchToggleByName : IQuery<Toggle>
    {
        public string ToggleName { get; }

        public FetchToggleByName(string toggleName)
        {
            ToggleName = toggleName;
        }
    }
}