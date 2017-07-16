using System.Collections.Generic;
using ToggleApi.Models;

namespace ToggleApi.Queries
{
    public interface IQueryHandler :
        IQueryHandler<FetchTogglesForClient, IEnumerable<KeyValuePair<string, bool>>>,
        IQueryHandler<FetchToggleByName, Toggle>
    {

    }
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery query);
    }
}
