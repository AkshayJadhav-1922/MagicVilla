using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models.Dto
{
    public class VillaUpdateDTO
    {
        //DTOs provide a wrapper between entities and database models and what is being exposed from the api
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }
        public string Details { get; set; }
        [Required]
        public double Rate { get; set; }
        [Required]
        public int Occupancy { get; set; }
        public int Sqft { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        public string Amenity { get; set; }
    }
}
