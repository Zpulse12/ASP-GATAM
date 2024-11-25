using System.Linq.Expressions;

namespace Gatam.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(params Expression<Func<T, object>>[] includes);
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<T?> FindById(string id);
        Task<T> FindByIdWithIncludes(string id, params Expression<Func<T, object>>[] includes);
        Task<T?> FindByProperty(string propertyName, string value);
        Task<T?> FindFirstAsync(Expression<Func<T, bool>> predicate);


    }
}
