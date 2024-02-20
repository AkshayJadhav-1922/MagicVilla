using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/Roles")]
    public class RolesController : Controller
    {
        private readonly IRolesRepository _rolesRepository;
        private APIResponse _response;
        public RolesController(IRolesRepository rolesRepository)
        {
            _rolesRepository = rolesRepository;
            _response = new APIResponse();
        }


        [HttpGet(Name ="GetRoles")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult Index()
        {
            var Roles = _rolesRepository.GetAllRole();
            if(Roles!=null && Roles.Count()>0)
            {
                _response.ISuccess = true;
                _response.Result = Roles;
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);
            }
            else
            {
                _response.ISuccess= false;
                _response.Result = null;
                _response.StatusCode = HttpStatusCode.BadRequest;
                return BadRequest(_response);
            }
        }
    }
}
