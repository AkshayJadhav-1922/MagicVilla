using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaNumberService
    {
        Task<T> GetAllAsync<T>(string tocken);
        Task<T> GetAsync<T>(int id, string tocken);
        Task<T> CreateAsync<T>(VillaNumberCreateDTO dto, string tocken);
        Task<T> UpdateAsync<T>(VillaNumberUpdateDTO dto, string tocken);
        Task<T> DeleteAsync<T>(int id, string tocken);
    }
}
