using MagicVilla_VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository.IRepository
{
    public interface IRolesRepository
    {
        public IEnumerable<Role> GetAllRole();
    }
}
