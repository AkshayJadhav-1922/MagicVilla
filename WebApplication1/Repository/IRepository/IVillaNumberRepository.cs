using MagicVilla_VillaAPI.Models;
using System.Linq.Expressions;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IVillaNumberRepository: IRepository<VillaNumber>
    {
        Task<VillaNumber> UpdateAsync(VillaNumber entiy);

    }
}
