using Microsoft.AspNetCore.Mvc;
using MVC_MPICTURE.Models.DTO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;

namespace MVC_MPICTURE.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager, IConfiguration configuration, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _configuration = configuration;
            _signInManager = signInManager;
        }

        public string CreateJWTToken(IdentityUser user, List<string> roles)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDTO model)
        {
            if (string.IsNullOrEmpty(model.Username) || model.Username.Length < 3)
            {
                ModelState.AddModelError("Username", "Tên tài khoản phải có ít nhất 3 ký tự.");
            }

            if (string.IsNullOrEmpty(model.Password) || model.Password.Length < 8)
            {
                ModelState.AddModelError("Password", "Mật khẩu phải có ít nhất 8 ký tự.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = new IdentityUser { UserName = model.Username, Email = model.Username };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                if (model.Roles != null && model.Roles.Any())
                {
                    var roleResult = await _userManager.AddToRolesAsync(user, model.Roles);
                    if (!roleResult.Succeeded)
                    {
                        foreach (var error in roleResult.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(model);
                    }
                }

                var jwtToken = CreateJWTToken(user, model.Roles.ToList());
                HttpContext.Session.SetString("JwtToken", jwtToken);
                return RedirectToAction("Login");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return View(model);
        }




        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model)
        {
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var roles = await _userManager.GetRolesAsync(user);
                var jwtToken = CreateJWTToken(user, roles.ToList());
                HttpContext.Session.SetString("JwtToken", jwtToken);

                await _signInManager.SignInAsync(user, isPersistent: false);

                return RedirectToAction("Index", "Images");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(model);
            }
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Remove("JwtToken");
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }

    }
}
