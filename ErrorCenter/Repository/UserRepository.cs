namespace Central_De_Erros.Repository
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Security.Principal;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading.Tasks;
    using System.Web;
    using Central_De_Erros.Models;
    using Central_De_Erros.ViewModel;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Primitives;
    using Microsoft.IdentityModel.Tokens;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class UserRepository : IUserRepository
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<IdentityUser> getUserByEmail(string email)
        {
            return await userManager.FindByEmailAsync(email);
        }

        public async Task<IdentityResult> IncludeUser(UserViewModel userView)
        {
            ApplicationUser user = DefineApplicationUser(userView);

            var userInDB = await getUserByEmail(user.Email);
            if (IsEmailIsAlreadyInDB(userInDB))
            {
                return IdentityResult.Failed(new IdentityError() { Code = "Invalid Email" , Description = "Email " + userView.Email + " is already in use." }) ;
            }
            return await this.userManager.CreateAsync(user, userView.Password);
        }

        public async Task<IdentityUser> Login(string email, string password)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user != null &&
                await this.userManager.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }

        public async Task<string> ForgotPassword(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return null;
            }
            else
            {
                return await this.userManager.GeneratePasswordResetTokenAsync(user); 
            }
        }

        public async Task<IdentityResult> ConfirmEmail(string userEmail, string code)
        {
            var user = (ApplicationUser) await getUserByEmail(userEmail);
            if(user != null)
            {
                return await userManager.ConfirmEmailAsync(user, code);
            }
            return IdentityResult.Failed(new IdentityError() { Code = "Email Doesn't Exist", Description = "Email " + userEmail + " doesn't exist." });
        }

        public async Task<IdentityResult> ChangePassword(string userEmail, string code, string password)
        {
            var user = (ApplicationUser) await getUserByEmail(userEmail);
            return await userManager.ResetPasswordAsync(user, code, password);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user)
        {
            return await userManager.GenerateEmailConfirmationTokenAsync((ApplicationUser)user);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(IdentityUser user)
        {
            return await userManager.GeneratePasswordResetTokenAsync((ApplicationUser)user);
        }

        public string GenerateUserJWTToken(LoginUserSucceeded user, AppSettings appSettings)
        {

            ClaimsIdentity identity = new ClaimsIdentity(
                    new GenericIdentity(user.id, "Login"),
                    new[] {
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email),
                        new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
                    }
                );

            DateTime dataCriacao = DateTime.Now;
            DateTime dataExpiracao = dataCriacao +
                TimeSpan.FromHours(appSettings.TimeSpan);

            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            var securityToken = handler.CreateToken(new SecurityTokenDescriptor
            {
                Issuer = appSettings.Issuer,
                Audience = appSettings.Audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Subject = identity,
                NotBefore = dataCriacao,
                Expires = dataExpiracao
            });
            return handler.WriteToken(securityToken);


        }

        private static ApplicationUser DefineApplicationUser(UserViewModel userView)
        {
            var user = new ApplicationUser()
            {
                UserName = userView.Name,
                FirstName = userView.Name,
                LastName = userView.LastName,
                Email = userView.Email,
                EmailConfirmed = false // para casos em que seja necessário enviar email de confirmação ao usuário, aqui estará false
            };
            return user;
        }

        private bool IsEmailIsAlreadyInDB(IdentityUser user)
        {
            return user != null;
        }

        
    }
}
