using System.Linq.Expressions;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaRepository: IRepository<Villa>
    {
        Task<Villa> UpdateAsync(Villa entiy);

    }
}
