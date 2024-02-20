using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace MagicVilla_VillaAPI.Repository
{
    public class RolesRepository : IRolesRepository
    {
        private readonly ApplicationDbContext _db;
        public RolesRepository(ApplicationDbContext db)
        {
            _db = db;
        }
        public IEnumerable<Role> GetAllRole()
        {
            return _db.Roles.ToList();
        }
    }
}
