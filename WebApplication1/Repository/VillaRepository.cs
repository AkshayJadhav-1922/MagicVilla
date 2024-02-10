using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using WebApplication1.Data;
using WebApplication1.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db): base(db)
        {
            _db = db;
        }
        
        public async Task<Villa> UpdateAsync(Villa entiy)
        {
            entiy.UpdatedDate = DateTime.Now;
            _db.Villas.Update(entiy);
            await SaveAsync();
            return entiy;
        }

    }
}
