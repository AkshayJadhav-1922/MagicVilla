using AutoMapper;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dto;
using System.Net;
using Microsoft.AspNetCore.Authorization;

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
        protected APIResponse _response;
        public VillaAPIController(IVillaRepository vill, IMapper mapper)
        {

            //_db = db;
            _vill = vill;
            _mapper = mapper;
            _response = new APIResponse();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                IEnumerable<Villa> listofVillas = await _vill.GetAllAsync();
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<List<VillaDTO>>(listofVillas);
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

        }

        [HttpGet("{id:int}", Name = "GetVilla")]
        //produesResponse basically removed 'undocumented' tag from endpoint in swager documentation
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[ProducesResponseType(StatusCodes.Status200OK, Type = typeof(VillaDTO))]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }
                var villa = await _vill.GetAsync(u => u.Id == id);
                if (villa == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ISuccess = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = villa;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }

        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVill([FromBody] VillaCreateDTO villaDTO)
        {
            //This is either way of doing validations, if you don't want it using APIController
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);
            try
            {
                if (await _vill.GetAsync(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
                {
                    _response.ISuccess = false;
                    _response.ErrorMessages = new List<string>()
                {
                    "Villa Name already exist"
                };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (villaDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                Villa model = _mapper.Map<Villa>(villaDTO);
                await _vill.CreateAsync(model);

                _response.Result = _mapper.Map<VillaDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;

                //will give 201 response
                return CreatedAtRoute("GetVilla", new { id = model.Id }, _response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                var VillToBeDeleted = await _vill.GetAsync(v => v.Id == id);
                if (VillToBeDeleted == null)
                {
                    _response.ISuccess = false;
                    _response.ErrorMessages = new List<string>()
                {
                    "Villa does not exist"
                };
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                await _vill.RemoveAsync(VillToBeDeleted);

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVill(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            try
            {
                if (villaDTO == null || id != villaDTO.Id)
                {
                    _response.ISuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                     return BadRequest(_response);
                }

                var VillToUpdate = await _vill.GetAsync(u => u.Id == id, false);
                if (VillToUpdate == null)
                {
                    _response.ISuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                Villa model = _mapper.Map<Villa>(villaDTO);

                await _vill.UpdateAsync(model);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }

        [HttpPatch("{id:int}", Name = "PatchVilla")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> PatchVill(int id, JsonPatchDocument<VillaUpdateDTO> villDto)
        {
            try
            {
                if (id == 0 || villDto == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                var VillToPatch = await _vill.GetAsync(u => u.Id == id, false);
                if (VillToPatch == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                VillaUpdateDTO model = _mapper.Map<VillaUpdateDTO>(VillToPatch);

                villDto.ApplyTo(model, ModelState);
                if (!ModelState.IsValid)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                Villa model1 = _mapper.Map<Villa>(model);
                await _vill.UpdateAsync(model1);

                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.ISuccess = false;
                _response.ErrorMessages = new List<string>()
                {
                    ex.Message.ToString()
                };
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
    }
}
