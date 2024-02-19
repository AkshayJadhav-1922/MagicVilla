using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = await _db.LocalUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && u.Password.ToLower() == loginRequestDTO.Password.ToLower());

            if (user == null)
            {
                return new LoginResponseDTO(){
                    User = null,
                    Tocken ="" 
                };
            }

            //if user was found, generate JWT Tocken
            var tockenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tockenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tocken = tockenHandler.CreateToken(tockenDescriptor);

            LoginResponseDTO loginResponseDTO = new()
            {
                Tocken = tockenHandler.WriteToken(tocken),
                User = user
            };

            return loginResponseDTO;

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
