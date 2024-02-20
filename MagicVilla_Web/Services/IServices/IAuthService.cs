using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IAuthService
    {
        Task<T> LogInAsync<T>(LoginRequestDTO loginRequest);
        Task<T> RegisterAsync<T>(RegitrationRequestDTO regitrationRequestDTO);
    }
}
