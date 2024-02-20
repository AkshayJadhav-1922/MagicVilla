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
using MagicVilla_VillaAPI.Models.Dto;
using Microsoft.AspNetCore.Authorization;

namespace WebApplication1.Controllers
{
    //if we are using mvc, it will have Controller instead of Controller base
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaAPINumberController : ControllerBase
    {
        //private readonly ApplicationDbContext _db;
        private readonly IVillaNumberRepository _villNumber;
        private readonly IVillaRepository _villa;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaAPINumberController(IVillaNumberRepository villNumber, IMapper mapper, IVillaRepository villa)
        {

            //_db = db;
            _villNumber = villNumber;
            _mapper = mapper;
            _response = new APIResponse();
            _villa = villa;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetVillaNumbers()
        {
            try
            {
                IEnumerable<VillaNumber> listofVillaNumbers = await _villNumber.GetAllAsync(includeProperty: "Villa");
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<List<VillaNumberDTO>>(listofVillaNumbers);
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

        [HttpGet("{id:int}", Name = "GetVillaNumber")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }
                var villaNumber = await _villNumber.GetAsync(u => u.VillNo == id, includeProperty: "Villa");
                if (villaNumber == null)
                {
                    _response.StatusCode = HttpStatusCode.NotFound;
                    _response.ISuccess = false;
                    return NotFound(_response);
                }
                _response.StatusCode = HttpStatusCode.OK;
                _response.Result = _mapper.Map<VillaNumberDTO>(villaNumber);
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
        [Authorize(Roles ="admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> CreateVillaNumber([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            //This is either way of doing validations, if you don't want it using APIController
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);
            try
            {
                if (await _villNumber.GetAsync(u => u.VillNo == villaNumberCreateDTO.VillNo) != null)
                {
                    _response.ISuccess = false;
                    _response.ErrorMessages = new List<string>()
                {
                    "Villa Number already exist"
                };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if(await _villa.GetAsync(u=> u.Id == villaNumberCreateDTO.VillId) == null)
                {
                    _response.ISuccess = false;
                    _response.ErrorMessages = new List<string>()
                {
                    "Villa Id does not exist"
                };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                if (villaNumberCreateDTO == null)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberCreateDTO);
                await _villNumber.CreateAsync(model);

                _response.Result = _mapper.Map<VillaNumberCreateDTO>(model);
                _response.StatusCode = HttpStatusCode.Created;

                //will give 201 response
                return CreatedAtRoute("GetVillaNumber", new { id = model.VillNo }, _response);
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

        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> DeleteVillaNumber(int id)
        {
            try
            {
                if (id == 0)
                {
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    _response.ISuccess = false;
                    return BadRequest(_response);
                }

                var VillToBeDeleted = await _villNumber.GetAsync(v => v.VillNo == id);
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
                await _villNumber.RemoveAsync(VillToBeDeleted);

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

        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [Authorize(Roles = "admin")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult<APIResponse>> UpdateVillNumber(int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdateDTO)
        {
            try
            {
                if (villaNumberUpdateDTO == null || id != villaNumberUpdateDTO.VillNo)
                {
                    _response.ISuccess = false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                     return BadRequest(_response);
                }

                if (await _villa.GetAsync(u => u.Id == villaNumberUpdateDTO.VillId) == null)
                {
                    _response.ISuccess = false;
                    _response.ErrorMessages = new List<string>()
                {
                    "Villa Id does not exist"
                };
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }

                var VillToUpdate = await _villNumber.GetAsync(u => u.VillNo == id, false);
                if (VillToUpdate == null)
                {
                    _response.ISuccess = false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }

                VillaNumber model = _mapper.Map<VillaNumber>(villaNumberUpdateDTO);

                await _villNumber.UpdateAsync(model);

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

        
    }
}
