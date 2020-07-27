using System.Text.Encodings.Web;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Central_De_Erros.Repository;
using Central_De_Erros.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Central_De_Erros.Controllers
{
    

    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;
        private readonly IEmailSender _emailSender;

        public UserController(
            IUserRepository repo, 
            IMapper mapper, 
            IOptions<AppSettings> appSettings,
            IEmailSender emailSender)
        {
            _repo = repo;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Creates an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/User
        ///     {
        ///         "name": "Antonio",
        ///         "lastName": "Almeida",
        ///         "email": "Antonio@Antonio.com",
        ///         "password": "Antonio@123",
        ///         "confirmThePassword": "Antonio@123"
        ///     }
        ///
        /// </remarks>
        /// <param name="user"></param>
        /// <returns>A new user was created</returns>
        /// <response code="200">User has been created</response>
        /// <response code="400">If the user is null</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<UserViewCreated>> Post([FromBody] UserViewModel user)
        {
            var result = await _repo.IncludeUser(user);
            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }
            else
            {
                var userDB = await _repo.getUserByEmail(user.Email);
                var token = await _repo.GenerateEmailConfirmationTokenAsync(userDB);
                await _emailSender.SendEmailAsync(user.Email, "Confirm your email", "Use this token to confirm your email: " + token);
                return Ok(_mapper.Map<UserViewCreated>(userDB));
            }
        }

        /// <summary>
        /// Email confirmation
        /// </summary>
        /// <response code="200">Your email has been confirmed</response>
        /// <response code="400">An error has occurred. The email still needs to be confirmed</response>            
        [HttpGet("ConfirmEmail")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> ConfirmEmail(string userEmail, string token)
        {
            if (userEmail == null || token == null)
            {
                return BadRequest("User email or token is invalid");
            }
            var result = await _repo.ConfirmEmail(userEmail, token);
            if (result.Succeeded)
            {
                return Ok("Your email has been confirmed");
            }
            return BadRequest(result.Errors);
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <response code="200">Successful login</response>
        /// <response code="400">Unsuccessful login</response>
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginUserSucceeded>> Login(LoginViewModel login)
        {
            IdentityUser identityUser = await _repo.Login(login.Email, login.Password);
            var user = _mapper.Map<LoginUserSucceeded>(identityUser);
            if(user == null)
            {
                return BadRequest("Invalid user or password");
            }
            else
            {
                user.Token = _repo.GenerateUserJWTToken(user, _appSettings);
                return Ok(user);
            }
            
        }

        /// <summary>
        /// Generates token that is sent to user's email to change password
        /// </summary>
        /// <response code="200">Token successfully sent</response>
        /// <response code="404">User email not found</response>
        [HttpGet("ForgotPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> ForgotPassword([FromQuery]ForgotPasswordViewModel view)
        {
            
            string token = await _repo.ForgotPassword(view.Email);

            if (token == null)
            {
                return NotFound("Doens't exist user with this email");     
            }

            var user = await _repo.getUserByEmail(view.Email);
            await _emailSender.SendEmailAsync(view.Email, "Reset your Password with this token", "Use this token to change your password: " + token);
            return Ok("An Email has been sendded to " + view.Email);
        }

        /// <summary>
        /// Enter the token to change the password
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/User/ResetPassword
        ///     {
        ///         "email": "Antonio",
        ///         "token": "123456",
        ///         "password": "Antonio123_",
        ///         "confirmThePassword": "Antonio123_"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Password has been changed</response>
        /// <response code="400">An error has occurred</response>
        [HttpPost("ResetPassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword(ResetPasswordView user)
        {
            var result = await _repo.ChangePassword(user.Email, user.Token, user.Password);
            if (result.Succeeded)
            {
                return Ok($"Password of'{user.Email}' has been changed");
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }






    }
}
