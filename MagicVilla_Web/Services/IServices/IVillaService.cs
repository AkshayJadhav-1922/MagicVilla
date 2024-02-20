using MagicVilla_Web.Models.Dto;

namespace MagicVilla_Web.Services.IServices
{
    public interface IVillaService
    {
        Task<T> GetAllAsycn<T>(string tocken);
        Task<T> GetAsync<T>(int id, string tocken);
        Task<T> CreateAsync<T>(VillaCreateDTO dto, string tocken);
        Task<T> UpdateAsync<T>(VillaUpdateDTO dto, string tocken);
        Task<T> DeleteAsync<T>(int id, string tocken);
    }
}
