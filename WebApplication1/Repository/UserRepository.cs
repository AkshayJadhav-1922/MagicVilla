using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _db;
        private string secretKey;
        public UserRepository(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSetting:Secret");

        }
        public bool IsUniqueUser(string username)
        {
            var user = _db.LocalUsers.FirstOrDefault(u => u.UserName == username);
            if (user == null)
            {
                return true;
            }
            return false;
        }

        public Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.LocalUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower() 
            && u.Password.ToLower()== loginRequestDTO.Password.ToLower());
            
            if (user == null)
            {
                return null;
            }

            //generating JWT tocken


        }

        public async Task<LocalUser> Register(RegitrationRequestDTO regitrationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = regitrationRequestDTO.UserName,
                Password = regitrationRequestDTO.Password,
                Name = regitrationRequestDTO.Name,
                Role = regitrationRequestDTO.Role
            };

            await _db.LocalUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            user.Password = "";
            return user;
        }
    }
}
