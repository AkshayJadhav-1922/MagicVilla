using System.ComponentModel.DataAnnotations;

namespace MagicVilla_Web.Models.Dto
{
    public class VillaNumberDTO
    {
        public int VillNo { get; set; }
        [Required]
        public int VillId { get; set; }
        public string SpecialDetails { get; set; }
    }
}
