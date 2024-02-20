using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;

namespace MagicVilla_Web.Services
{
    public class VillaNumberService : BaseService, IVillaNumberService
    {
        private readonly IHttpClientFactory _clientFactory;
        private string villUrl;

        public VillaNumberService(IHttpClientFactory clientFactory, IConfiguration configuration) : base(clientFactory)
        {
            _clientFactory = clientFactory;
            villUrl = configuration.GetValue<string>("ServiceUrls:VillaAPI");
        }
        public Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string tocken)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = dto,
                Url = villUrl + "/api/v1/VillaNumberAPI/",
                Tocken = tocken
            });
        }

        public Task<T> DeleteAsync<T>(int id, string tocken)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.DELETE,
                Url = villUrl + "/api/v1/VillaNumberAPI/" + id,
                Tocken = tocken
            });
        }

        public Task<T> GetAllAsync<T>(string tocken)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villUrl + "/api/v1/VillaNumberAPI/",
                Tocken = tocken
            });
        }

        public Task<T> GetAsync<T>(int id, string tocken)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.GET,
                Url = villUrl + "/api/v1/VillaNumberAPI/" + id,
                Tocken = tocken
            });
        }

        public Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string tocken)
        {
            return SendAsync<T>(new APIRequest()
            {
                ApiType = SD.ApiType.PUT,
                Data = dto,
                Url = villUrl + "/api/v1/VillaNumberAPI/" + dto.VillNo,
                Tocken = tocken
            });
        }
    }
}
