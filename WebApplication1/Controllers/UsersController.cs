using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.Dto;
using MagicVilla_VillaAPI.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace MagicVilla_VillaAPI.Controllers
{
    [Route("api/v{version:apiVersion}/UserAuth")]
    [ApiController]
    [ApiVersionNeutral]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private APIResponse _response;
        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
            _response = new APIResponse();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO model)
        {
            
            var loginResponse = await _userRepository.Login(model);
            if(loginResponse.User == null || loginResponse.Tocken == "")
            {
                _response.ErrorMessages = new List<string>() { "User Name or password is incorrect" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ISuccess = false;
                return BadRequest(_response);
            }

            _response.StatusCode = HttpStatusCode.OK;
            _response.ISuccess = true;
            _response.Result = loginResponse;
            return Ok(_response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegitrationRequestDTO model)
        {
            var isUserNameUnique = _userRepository.IsUniqueUser(model.UserName);
            if (!isUserNameUnique)
            {
                _response.ErrorMessages= new List<string>() { "Enter Unique user name, as entered one already exist in system" };
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ISuccess = false;
                return BadRequest(_response);
            }

            var user = _userRepository.Register(model);
            if (user == null)
            {
                _response.ErrorMessages.Add("Error while registering");
                _response.StatusCode = HttpStatusCode.BadRequest;
                _response.ISuccess = false;
                return BadRequest(_response);
            }
            _response.StatusCode = HttpStatusCode.OK;
            _response.ISuccess = true;
            _response.Result = user;
            return Ok(_response);
        }
    }
}
