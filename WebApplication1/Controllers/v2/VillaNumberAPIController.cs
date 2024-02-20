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

namespace MagicVilla_VillaAPI.Controllers.v2
{
    //if we are using mvc, it will have Controller instead of Controller base
    [Route("api/v{version:apiVersion}/VillaNumberAPI")]
    [ApiController]
    [ApiVersion("2.0")]
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
        public IEnumerable<string> Get()
        {
            return new string[] { "string1", "string2" };
        }


    }
}
