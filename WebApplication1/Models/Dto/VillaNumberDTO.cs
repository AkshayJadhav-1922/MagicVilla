using System.ComponentModel.DataAnnotations;
using WebApplication1.Models.Dto;

namespace MagicVilla_VillaAPI.Models.Dto
{
    public class VillaNumberDTO
    {
        public int VillNo { get; set; }
        [Required]
        public int VillId { get; set; }
        public string SpecialDetails { get; set; }
        public VillaDTO villa { get; set; }
    }
}
