namespace Central_De_Erros.Repository
{
    using System.Threading.Tasks;
    using Central_De_Erros.Models;
    using Central_De_Erros.ViewModel;
    using Microsoft.AspNetCore.Identity;

    public interface IUserRepository
    {
        Task<IdentityUser> getUserByEmail(string email);

        Task<IdentityResult> IncludeUser(UserViewModel userView);

        Task<IdentityUser> Login(string email, string password);

        Task<string> ForgotPassword(string email);

        Task<IdentityResult> ConfirmEmail(string userEmail, string code);

        Task<IdentityResult> ChangePassword(string userEmail, string code, string password);

        string GenerateUserJWTToken(LoginUserSucceeded user, AppSettings appSettings);

        Task<string> GenerateEmailConfirmationTokenAsync(IdentityUser user);

        Task<string> GeneratePasswordResetTokenAsync(IdentityUser user);
    }
}
