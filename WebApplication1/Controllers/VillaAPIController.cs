﻿using Microsoft.AspNetCore.JsonPatch;
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
        private readonly ApplicationDbContext _db;
        public VillaAPIController(ApplicationDbContext db)
        {

            _db = db;

        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
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
            var villa = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villa == null)
                return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> CreateVill([FromBody] VillaDTO villaDTO)
        {
            //This is either way of doing validations, if you don't want it using APIController
            //if(!ModelState.IsValid)
            //    return BadRequest(ModelState);
            if (_db.Villas.FirstOrDefault(u => u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custome Error", "Vill Name already exist");
                return BadRequest(ModelState);
            }

            if (villaDTO == null)
                return BadRequest(villaDTO);

            if (villaDTO.Id > 0)
                return StatusCode(StatusCodes.Status500InternalServerError);

            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                CreatedDate = DateTime.Now,
                UpdatedDate = DateTime.Now
            };
            
            _db.Villas.Add(model);
            _db.SaveChanges();

            //will give 201 response
            return CreatedAtRoute("GetVilla", new { id = villaDTO.Id }, villaDTO);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if (id == 0)
                return BadRequest();

            var VillToBeDeleted = _db.Villas.FirstOrDefault(v => v.Id == id);
            if (VillToBeDeleted == null)
            {
                ModelState.AddModelError("Error", "Vill do not exist");
                return NotFound(ModelState);
            }
            _db.Villas.Remove(VillToBeDeleted);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public IActionResult UpdateVill(int id, [FromBody] VillaDTO villaDTO)
        {
            if (id == 0 || id != villaDTO.Id)
                return BadRequest();

            var villaToUpdate = _db.Villas.FirstOrDefault(u => u.Id == id);
            if (villaToUpdate == null)
                return NotFound();


            Villa model = new()
            {
                Amenity = villaDTO.Amenity,
                Details = villaDTO.Details,
                ImageUrl = villaDTO.ImageUrl,
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,
                UpdatedDate = DateTime.Now
            };
            _db.Villas.Update(model);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}", Name ="PatchVilla")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public ActionResult PatchVill(int id, JsonPatchDocument<VillaDTO> villDto)
        {
            if(id==0 || villDto == null)
                return BadRequest();

            var VillToPatch = _db.Villas.AsNoTracking().FirstOrDefault(u=> u.Id == id);
            if (VillToPatch == null)
                return BadRequest();

            VillaDTO model = new()
            {
                Amenity = VillToPatch.Amenity,
                Details = VillToPatch.Details,
                ImageUrl = VillToPatch.ImageUrl,
                Id = VillToPatch.Id,
                Name = VillToPatch.Name,
                Occupancy = VillToPatch.Occupancy,
                Rate = VillToPatch.Rate,
                Sqft = VillToPatch.Sqft
            };
            villDto.ApplyTo(model, ModelState);
            if(!ModelState.IsValid)
                return BadRequest(ModelState);


            Villa model1 = new()
            {
                Amenity = model.Amenity,
                Details = model.Details,
                ImageUrl = model.ImageUrl,
                Id = model.Id,
                Name = model.Name,
                Occupancy = model.Occupancy,
                Rate = model.Rate,
                Sqft = model.Sqft,
                UpdatedDate = DateTime.Now
            };
            _db.Villas.Update(model1);
            _db.SaveChanges();

            return NoContent();
        }
    }
}
