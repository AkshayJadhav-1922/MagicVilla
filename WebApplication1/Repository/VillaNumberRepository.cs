using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaNumberRepository : Repository<VillaNumber>, IVillaNumberRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaNumberRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        
        public async Task<VillaNumber> UpdateAsync(VillaNumber entiy)
        {
            entiy.UpdatedDate = DateTime.Now;
            _db.VillaNumbers.Update(entiy);
            await SaveAsync();
            return entiy;
        }

    }
}
