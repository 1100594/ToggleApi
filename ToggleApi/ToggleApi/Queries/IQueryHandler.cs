using System.Collections.Generic;

namespace ToggleApi.Queries
{
    public interface IQueryHandler: IQueryHandler <FetchTogglesForClient, IEnumerable<KeyValuePair<string, bool>>>
    {
        
    }
    public interface IQueryHandler<in TQuery, out TResult> where TQuery : IQuery<TResult>
    {
        TResult Execute(TQuery query);
    }
}
