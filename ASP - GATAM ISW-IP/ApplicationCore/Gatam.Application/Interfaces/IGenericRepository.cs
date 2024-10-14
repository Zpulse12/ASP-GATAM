using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gatam.Application.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> Create(T entity);
        Task<T> Update(T entity);
        Task Delete(T entity);
        Task<T?> FindById(string id);
        Task<T> FindByName(string name);

    }
}
