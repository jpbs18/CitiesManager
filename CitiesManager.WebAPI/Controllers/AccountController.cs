using CitiesManager.Core.DTO;
using CitiesManager.Core.Identity;
using CitiesManager.Core.ServiceContracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CitiesManager.WebAPI.Controllers
{
    // requests to this controller are accepted without authentication being needed
    [AllowAnonymous]
    public class AccountController : CustomControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly IJwtService _jwtService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<ApplicationRole> roleManager, IJwtService jwtService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _jwtService = jwtService;
        }


        [HttpPost("/register")]
        public async Task<ActionResult<ApplicationUser>> Post(RegisterDTO registerDTO)
        {
            if (!ModelState.IsValid)
            {
                string errors = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(errors);
            }

            ApplicationUser user = new()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                PhoneNumber = registerDTO.PhoneNumber,
                PersonName = registerDTO.PersonName,
            };

            IdentityResult result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                AuthenticationResponse response = _jwtService.CreateJwtToken(user);

                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpirationDate = response.RefreshTokenExpirationDate;
                await _userManager.UpdateAsync(user);

                return Ok(response);
            }

            string errorsMessage = string.Join(" | ", result.Errors.Select(e => e.Description));
            return Problem(errorsMessage);
        }


        [HttpPost("/login")]
        public async Task<IActionResult> Login(LoginDTO loginDto)
        {
            if (!ModelState.IsValid)
            {
                string error = string.Join(" | ", ModelState.Values.SelectMany(x => x.Errors).Select(e => e.ErrorMessage));
                return Problem(error);
            }

            var result = await _signInManager.PasswordSignInAsync(loginDto.Email, loginDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                ApplicationUser? user = await _userManager.FindByEmailAsync(loginDto.Email);

                if(user is null)
                {
                    return NoContent();
                }

                AuthenticationResponse response = _jwtService.CreateJwtToken(user);

                user.RefreshToken = response.RefreshToken;
                user.RefreshTokenExpirationDate = response.RefreshTokenExpirationDate;
                await _userManager.UpdateAsync(user);

                return Ok(response);
            }

            return Problem("Invalid credentials");
        }


        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return NoContent();
        }


        [HttpGet]
        public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
        {
            ApplicationUser? user = await _userManager.FindByEmailAsync(email);
            return Ok(user == null);
        }
    }
}
