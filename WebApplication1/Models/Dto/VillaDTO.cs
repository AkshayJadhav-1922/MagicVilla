using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Dto
{
    public class VillaDTO
    {
        //DTOs provide a wrapper between entities and database models and what is being exposed from the api
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
    }
}
