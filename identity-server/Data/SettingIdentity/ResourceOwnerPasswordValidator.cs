using System.Linq;
using System.Threading.Tasks;
using Identity_server.Data.DomainModel;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Identity_server.Data.SettingIdentity
{
    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
    {
        private readonly UserManager<User> _userManager;

        public ResourceOwnerPasswordValidator(UserManager<User>  userManager)
        {
            _userManager = userManager;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(x => x.UserName == context.UserName);
            
            if (user != null)
            {
                var userManager = await _userManager.CheckPasswordAsync(user, context.Password);
                if (userManager)
                 context.Result = new GrantValidationResult(user.Id.ToString(),
                     OidcConstants.AuthenticationMethods.Password);
            }
        }
    }
}