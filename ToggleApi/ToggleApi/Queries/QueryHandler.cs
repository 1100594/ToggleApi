using System.Collections.Generic;
using ToggleApi.Models;
using ToggleApi.Repository;

namespace ToggleApi.Queries
{
    public class QueryHandler: IQueryHandler
    {
        private readonly IToggleClientRepository _repository;

        public QueryHandler(IToggleClientRepository repository)
        {
            _repository = repository;
        }

        public IEnumerable<KeyValuePair<string, bool>> Execute(FetchTogglesForClient query)
        {
            return _repository.GetTogglesForClient(query.ClientId, query.ClientVersion);
        }

        public Toggle Execute(FetchToggleByName query)
        {
            return _repository.GetToggleByName(query.ToggleName);
        }
    }
}
