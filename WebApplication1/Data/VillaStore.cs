using WebApplication1.Models.Dto;

namespace WebApplication1.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>()
            {
                new VillaDTO() {Id=1 , Name="Pool Villa"},
                new VillaDTO() {Id=2 , Name="Beach Villa"}
            };
    }
}
