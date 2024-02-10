using System.Linq.Expressions;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null);
        Task<T> GetAsync(Expression<Func<T, bool>> filter = null, bool tracked = true);
        Task CreateAsync(T entiy);
        Task RemoveAsync(T entiy);
        Task SaveAsync();
    }
}
