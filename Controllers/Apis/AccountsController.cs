using KindergartenManagementSystem.Controllers.Apis.Dtos;
using KindergartenManagementSystem.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;


namespace KindergartenManagementSystem.Controllers.Apis
{
    [Route("api/accounts")]
    public class AccountsController : Controller
    {
        private readonly IUserManager _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ISignInManager _signInManager;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountsController(IUserManager userManager, IUnitOfWork unitOfWork, IEmailService emailService, ISignInManager signInManager, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _signInManager = signInManager;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpGet("resetPassword")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
                return NotFound();

            if (!await _userManager.IsEmailConfirmedAsync(user))
                return BadRequest("not confirmed");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _emailService.SendEmailAsync(resetPasswordDto.Email, "Reset password",
                $"Please reset your password by use this code: {token}.");
            if (!result.Succeeded)
                return BadRequest("Failed send message to your email, please try again.");

            return Ok("We send reset password code to your email.");
        }

        [HttpPost("resetPasswordConfirmation")]
        public async Task<IActionResult> ResetPasswordConfirmation(
            [FromBody] ResetPasswordConfirmationDto resetPasswordConfirmationDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(resetPasswordConfirmationDto.Email);
            if (user == null)
                return NotFound();

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordConfirmationDto.Token,
                resetPasswordConfirmationDto.Password);
            if (!result.Succeeded)
                return BadRequest("Failed to reset password, please try again.");

            if (await _unitOfWork.SaveChangesAsync() <= 0)
                return BadRequest("Failed to reset password, please try again.");

            return Ok("password had been reset successfully.");
        }

        [HttpPost("signIn")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByEmailAsync(signInDto.Email);
            if (user == null)
                return BadRequest("username or password incorrect.");
            else if (!await _signInManager.CheckPasswordAsync(user, signInDto.Password))
                return BadRequest("username or password incorrect.");

            if (_signInManager.IsSignInRequireConfirmedEmail())
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return BadRequest("SignIn require confirmed email.");

            try
            {
                var jwt = await _signInManager.GenerateAuthenticationTokenAsync(user);
                if (await _unitOfWork.SaveChangesAsync() <= 0)
                    return StatusCode(StatusCodes.Status500InternalServerError);

                var jwtDto = _mapper.Map<JwtDto>(jwt);
                return Ok(jwtDto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("signOut")]
        [Authorize]
        public async Task<IActionResult> SignOut(SignOutDto signOutDto)
        {

            if (!signOutDto.FromAllDevices)
            {
                var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
                await _signInManager.InvalidateToken(token, null, false);

                if (await _unitOfWork.SaveChangesAsync() > 0)
                    return Ok();

                return StatusCode(StatusCodes.Status500InternalServerError);
            }

            var userEmail = _httpContextAccessor.HttpContext.User.Claims.Single(c => c.Type == "UserName").Value;
            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
                return NotFound();

            await _signInManager.InvalidateToken(string.Empty, user, true);
            if (await _unitOfWork.SaveChangesAsync() > 0)
                return Ok();

            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpGet("testAuthorize")]
        [Authorize]
        public IActionResult TestAuthorize()
        {
            return Ok("sdasdasda");
        }
    }
}
