using System.Collections.Generic;
using System.Threading.Tasks;

namespace YourNamespace.Shared.Repos.Contracts
{
    public interface IBaseRepository<TModel>
        where TModel : class
    {
        Task<IEnumerable<TModel>> GetAllAsync();
        Task<TModel> GetByIdAsync(int id);
        Task CreateAsync(TModel model);
        Task UpdateAsync(TModel model);
        Task SaveAsync(TModel model);
        Task DeleteAsync(int id);
        Task<bool> ExistsByIdAsync(int id);
        Task<IEnumerable<TModel>> GetWithPaginationAsync(int pageSize, int pageNumber);
    }
}
