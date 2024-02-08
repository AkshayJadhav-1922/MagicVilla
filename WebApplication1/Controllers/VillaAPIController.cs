using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dto;

namespace WebApplication1.Controllers
{
    //if we are using mvc, it will have Controller instead of Controller base
    [Route("api/VillaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        //produesResponse basically removed 'undocumented' tag from endpoint in swager documentation
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null)
                return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVill([FromBody]VillaDTO villaDTO)
        {
            //This is either way of doing validations, if you don't want it using APIController
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);

            if(villaDTO == null)
                return BadRequest(villaDTO);

            if (villaDTO.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            villaDTO.Id = VillaStore.villaList.OrderByDescending(u=> u.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villaDTO);

            //will give 201 response
            return CreatedAtRoute("GetVilla", new { id= villaDTO.Id } ,villaDTO);
        }
    }
}
