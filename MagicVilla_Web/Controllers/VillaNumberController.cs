using AutoMapper;
using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Models.VM;
using MagicVilla_Web.Services;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Reflection;

namespace MagicVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IVillaNumberService _villaNumberService;
        private readonly IVillaService _villaService;
        public VillaNumberController(IMapper mapper, IVillaNumberService villaNumberService, IVillaService villaService)
        {
            _mapper = mapper;
            _villaNumberService = villaNumberService;
            _villaService = villaService;
        }
        public async Task<ActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> list = new();
            var response = await _villaNumberService.GetAllAsync<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        [Authorize(Roles = "admin")]
        public async Task<ActionResult> CreateVillaNumber()
        {
            VillaNumberCreateVM villaNumberCreateVM = new VillaNumberCreateVM();

            var response = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));
            if (response != null && response.IsSuccess)
            {
                villaNumberCreateVM.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(response.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(villaNumberCreateVM);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateVillaNumber(VillaNumberCreateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.CreateAsync<APIResponse>(model.villaNumber, HttpContext.Session.GetString(SD.SessionTocken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Vill Number Created Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    TempData["error"] = "Something went wrong";
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
            var res = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));

            if (res != null && res.IsSuccess)
            {
                model.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> UpdateVillaNumber(int villaNo)
        {
            if (ModelState.IsValid)
            {
                VillaNumberUpdateVM villaNumberUpdateVM = new();
                var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionTocken));
                if (response != null && response.IsSuccess)
                {
                    VillaNumberDTO villatoUpdate = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                    villaNumberUpdateVM.villaNumber = _mapper.Map<VillaNumberUpdateDTO>(villatoUpdate);
                }
                var res = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));

                if (res != null && res.IsSuccess)
                {
                    villaNumberUpdateVM.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                        (Convert.ToString(res.Result)).Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }
                return View(villaNumberUpdateVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVillaNumber(VillaNumberUpdateVM model)
        {
            if (ModelState.IsValid)
            {
                var response = await _villaNumberService.UpdateAsync<APIResponse>(model.villaNumber, HttpContext.Session.GetString(SD.SessionTocken));
                if (response != null && response.IsSuccess)
                {
                    TempData["success"] = "Vill Number Update Successfully";
                    return RedirectToAction(nameof(IndexVillaNumber));
                }
                else
                {
                    TempData["error"] = "Something went wrong";
                    ModelState.AddModelError("ErrorMessages", response.ErrorMessages.FirstOrDefault());
                }
            }
            var res = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));

            if (res != null && res.IsSuccess)
            {
                model.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteVillaNumber(int villaNo)
        {
            if (ModelState.IsValid)
            {
                VillaNumberDeleteVM villaNumberDeleteVM = new();
                var response = await _villaNumberService.GetAsync<APIResponse>(villaNo, HttpContext.Session.GetString(SD.SessionTocken));
                if (response != null && response.IsSuccess)
                {
                    villaNumberDeleteVM.villaNumber = JsonConvert.DeserializeObject<VillaNumberDTO>(Convert.ToString(response.Result));
                }
                var res = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));

                if (res != null && res.IsSuccess)
                {
                    villaNumberDeleteVM.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                        (Convert.ToString(res.Result)).Select(i => new SelectListItem
                        {
                            Text = i.Name,
                            Value = i.Id.ToString()
                        });
                }
                return View(villaNumberDeleteVM);
            }
            return NotFound();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteVillaNumber(VillaNumberDeleteVM model)
        {
            var response = await _villaNumberService.DeleteAsync<APIResponse>(model.villaNumber.VillNo, HttpContext.Session.GetString(SD.SessionTocken));
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Vill Number deleted Successfully";
                return RedirectToAction(nameof(IndexVillaNumber));
            }
            TempData["error"] = "Something went wrong";

            var res = await _villaService.GetAllAsycn<APIResponse>(HttpContext.Session.GetString(SD.SessionTocken));

            if (res != null && res.IsSuccess)
            {
                model.villaList = JsonConvert.DeserializeObject<List<VillaDTO>>
                    (Convert.ToString(res.Result)).Select(i => new SelectListItem
                    {
                        Text = i.Name,
                        Value = i.Id.ToString()
                    });
            }
            return View(model);
        }

    }
}
