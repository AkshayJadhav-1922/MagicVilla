using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class RolesService : BaseService, IRolesService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private string ApiPath;

        public RolesService(IHttpClientFactory httpClientFactory, IConfiguration configuration): base(httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
            ApiPath = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> GetAllRoles<T>()
        {
            return SendAsync<T>(new APIRequest
            {
                ApiType = SD.ApiType.GET,
                Url = ApiPath + "/api/v1/Roles"
            });
        }
    }
}
