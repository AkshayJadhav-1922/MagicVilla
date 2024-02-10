using AutoMapper;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        //private readonly ApplicationDbContext _db;
        private readonly IVillaRepository _vill;
        private readonly IMapper _mapper;
        public VillaAPIController(IVillaRepository vill, IMapper mapper)
        {

            //_db = db;
            _vill = vill;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            IEnumerable<Villa> listofVillas = await _vill.GetAllAsync(); 
            return Ok(_mapper.Map<List<VillaDTO>>(listofVillas));
        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        //produesResponse basically removed 'undocumented' tag from endpoint in swager documentation
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0)
                return BadRequest();
            var villa = await _vill.GetAsync(u => u.Id == id);
            if (villa == null)
                return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> CreateVill([FromBody] VillaCreateDTO villaDTO)
        {
            //This is either way of doing validations, if you don't want it using APIController
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);
            if (await _vill.GetAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custome Error", "Vill Name already exist");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
                return BadRequest(villaDTO);

            Villa model = _mapper.Map<Villa>(villaDTO);
            
            
            await _vill.CreateAsync(model);

            //will give 201 response
            return CreatedAtRoute("GetVilla", new { id = model.Id }, model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var VillToBeDeleted = await _vill.GetAsync(v => v.Id == id);
            if (VillToBeDeleted == null)
            {
                ModelState.AddModelError("Error", "Vill do not exist");
                return NotFound(ModelState);
            }
            await _vill.RemoveAsync(VillToBeDeleted);

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> UpdateVill(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
                return BadRequest();

            var VillToUpdate = await _vill.GetAsync(u => u.Id == id, false);
            if (VillToUpdate == null)
                return NotFound();

            Villa model = _mapper.Map<Villa>(villaDTO);
            
            await _vill.UpdateAsync(model);

            return NoContent();
        }

        [HttpPatch("{id:int}", Name ="PatchVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PatchVill(int id, JsonPatchDocument<VillaUpdateDTO> villDto)
        {
            if(id==0 || villDto == null)
                return BadRequest();

            var VillToPatch = await _vill.GetAsync(u => u.Id == id, false);
            if (VillToPatch == null)
                return BadRequest();

            VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(VillToPatch);
            
            villDto.ApplyTo(model, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);

            Villa model1 = _mapper.Map<Villa>(model);
            await _vill.UpdateAsync(model1);

            return NoContent();
        }
    }
}
