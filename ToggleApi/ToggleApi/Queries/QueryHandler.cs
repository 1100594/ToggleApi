using System.Collections.Generic;
using ToggleApi.Models;
using ToggleApi.Repository;

namespace ToggleApi.Queries
{
    public class QueryHandler: IQueryHandler<FetchTogglesForClient, IEnumerable<Toggle>>
    {
        private readonly IToggleClientRepository _repository;

        public QueryHandler(IToggleClientRepository repository)
        {
            this._repository = repository;
        }

        public IEnumerable<Toggle> Execute(FetchTogglesForClient query)
        {
            return _repository.GetTogglesForClient(query.ClientId, query.ClientVersion);
        }
    }
}
