﻿using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace MagicVilla_Web.Controllers
{
    public class VillaController: Controller
    {
        private readonly IVillaService _villaService;
        private readonly IMapper _mapper;
        public VillaController(IVillaService villaService, IMapper mapper)
        {
            _mapper = mapper;
            _villaService = villaService;
        }
        public async Task<ActionResult> IndexVilla()
        {
            List<VillaDTO> list = new();
            var response = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));
            if(response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles ="admin")]
        public async Task<ActionResult> CreateVilla()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVilla(VillaCreateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.CreateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionTocken));
                if(response != null && response.IsSuccess)
                {
                    TempData["success"] = "Vill created Successfully";

                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Something went wrong";

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVilla(int villId)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.GetAsync<APIResponse>(villId, HttpContext.Session.GetString(SD.SessionTocken));
                if(response !=null && response.IsSuccess)
                {
                    VillaDTO villatoUpdate = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                    return View(_mapper.Map<VillaUpdateDTO>(villatoUpdate));
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO model)
        {
            if(ModelState.IsValid)
            {
                var response = await _villaService.UpdateAsync<APIResponse>(model, HttpContext.Session.GetString(SD.SessionTocken));
                if(response!= null && response.IsSuccess)
                {
                    TempData["success"] = "Villa Updated Successfully";

                    return RedirectToAction(nameof(IndexVilla));
                }
            }
            TempData["error"] = "Something went wrong";

            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVilla(int villId)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaService.GetAsync<APIResponse>(villId, HttpContext.Session.GetString(SD.SessionTocken));
                if (response != null && response.IsSuccess)
                {
                    VillaDTO villatoDelete = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
                    return View(villatoDelete);
                }
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVilla(VillaDTO model)
        {
            var response = await _villaService.DeleteAsync<APIResponse>(model.Id, HttpContext.Session.GetString(SD.SessionTocken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Villa Deleted Successfully";

                return RedirectToAction(nameof(IndexVilla));
            }
            TempData["error"] = "Something went wrong";

            return View(model);
        }
    }
}
