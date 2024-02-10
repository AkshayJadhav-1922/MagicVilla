using System.Linq.Expressions;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository
    {
        Task<List<Villa>> GetAll(Expression<Func<Villa, bool>> filter = null);
        Task<Villa> Get(Expression<Func<Villa, bool>> filter = null, bool tracked = true);
        Task Create(Villa entiy);
        Task Remove(Villa entiy);
        Task Save();

    }
}
