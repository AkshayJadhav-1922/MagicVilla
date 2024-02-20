using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Identity.Data;

namespace MagicVilla_Web.Services
{
    public class AuthService : BaseService, IAuthService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string villaUrl;
        public AuthService(IHttpClientFactory httpClientFactory, IConfiguration configuration): base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            villaUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> LogInAsync<T>(LoginRequestDTO loginRequest)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = loginRequest,
                Url = villaUrl + "/api/UserAuth/login"
            });
        }

        public Task<T> RegisterAsync<T>(RegitrationRequestDTO regitrationRequestDTO)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = regitrationRequestDTO,
                Url = villaUrl + "/api/UserAuth/register"
            });
        }
    }
}
