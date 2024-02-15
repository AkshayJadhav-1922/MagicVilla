using MagicVilla_Web.Models.Dto;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MagicVilla_Web.Models.VM
{
    public class VillaNumberDeleteVM
    {
        public VillaNumberDeleteVM()
        {
            villaNumber = new VillaNumberDTO();
        }
        public VillaNumberDTO villaNumber { get; set; }
        [ValidateNever]
        public IEnumerable<SelectListItem> villaList { get; set; }
    }
}
