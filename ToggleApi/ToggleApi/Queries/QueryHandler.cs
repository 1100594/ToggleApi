using System.Collections.Generic;
using ToggleApi.Repository;

namespace ToggleApi.Queries
{
    public class QueryHandler: IQueryHandler
    {
        private readonly IToggleClientRepository _repository;

        public QueryHandler(IToggleClientRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<KeyValuePair<string, bool>> Execute(FetchTogglesForClient query)
        {
            return _repository.GetTogglesForClient(query.ClientId, query.ClientVersion);
        }
    }
}
