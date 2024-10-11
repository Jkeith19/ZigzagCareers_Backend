using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microservice.Models;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.String;


namespace Microservice.Services
{
    public class UserProfileService : IProfileService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserProfileService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <inheritdoc /> 
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subClaimValue = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            var user = await _userManager.FindByIdAsync(subClaimValue!) ?? throw new ArgumentException("Invalid sub claim");

            context.IssuedClaims =
            [
                new(JwtClaimTypes.Subject, user.Id),
                new(JwtClaimTypes.PreferredUserName, user.UserName!),
                new(JwtRegisteredClaimNames.UniqueName, user.UserName!)
            ];
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
            var subClaimValue = subject.Claims.FirstOrDefault(x => x.Type == "sub")?.Value;

            var user = await _userManager.FindByIdAsync(subClaimValue!);
            context.IsActive = false;

            if (user != null)
            {
                if (_userManager.SupportsUserSecurityStamp)
                {
                    var stamp = subject.Claims.FirstOrDefault(x => x.Type == "security_stamp")?.Value;
                    if (!IsNullOrWhiteSpace(stamp))
                    {
                        var securityStampFromDatabase = await _userManager.GetSecurityStampAsync(user);
                        if (stamp != securityStampFromDatabase)
                            return;
                    }
                }

                context.IsActive = !user.LockoutEnabled || !user.LockoutEnd.HasValue || user.LockoutEnd < DateTime.UtcNow;
            }
        }
    }
}
