namespace SmartUI.Grid.Services
{
    using SmartUI.Grid.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IHttpFeatureService<TModel>
            where TModel : class
    {
        Task<PagedResponse<TModel>> GetAsync(string url, FilterationData filterQuery = null);
        Task<IEnumerable<TModel>> GetDataAsync(string url);
        Task<IEnumerable<TModel>> GetByPredicateAsync(string url, List<FilterRuleModel> queryFilterRules);
    }
}
