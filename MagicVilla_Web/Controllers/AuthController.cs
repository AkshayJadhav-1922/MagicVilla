using MagicVilla_Utility;
using MagicVilla_Web.Models;
using MagicVilla_Web.Models.Dto;
using MagicVilla_Web.Services.IServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Security.Claims;

namespace MagicVilla_Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IRolesService _rolesService;
        public AuthController(IAuthService authService, IRolesService rolesService)
        {
            _authService = authService;
            _rolesService = rolesService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDTO obj = new();
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginRequestDTO obj)
        {
            APIResponse response = await _authService.LogInAsync<APIResponse>(obj);
            if(response!= null && response.IsSuccess)
            {
                LoginResponseDTO result = JsonConvert.DeserializeObject<LoginResponseDTO>(Convert.ToString(response.Result));
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, result.User.Name));
                identity.AddClaim(new Claim(ClaimTypes.Role, result.User.Role));
                var principal = new ClaimsPrincipal(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString(SD.SessionTocken, result.Tocken);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ModelState.AddModelError("CustomError", response.ErrorMessages.FirstOrDefault());
                return View(obj);

            }
        }

        [HttpGet]
        public async Task<IActionResult> Register() 
        {
            APIResponse result = await _rolesService.GetAllRoles<APIResponse>();
            if(result!=null && result.IsSuccess) 
            {
                IEnumerable<RolesDTO> roles = JsonConvert.DeserializeObject<IEnumerable<RolesDTO>>(Convert.ToString(result.Result));
                ViewBag.RolesList = roles.Select(i => i.RoleName).Select(x => new SelectListItem
                {
                    Text = x,
                    Value = x
                });
                return View();
            }
            else
            {
                ModelState.AddModelError("custome error", "Something went wrong while fetching roles");
                return View(ModelState);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegitrationRequestDTO obj)
        {
            APIResponse result = await _authService.RegisterAsync<APIResponse>(obj);
            if(result != null && result.IsSuccess)
            {
                RedirectToAction("Login");
            }
            return View(obj);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString(SD.SessionTocken, "");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
