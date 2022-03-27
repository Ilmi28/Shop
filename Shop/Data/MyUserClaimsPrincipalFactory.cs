using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using Shop.Models;

namespace Shop.Data
{
    public class MyUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser>
    {
        public MyUserClaimsPrincipalFactory(UserManager<AppUser> userManager, IOptions<IdentityOptions> options) : base(userManager, options) { }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("Name", user.Name));
            identity.AddClaim(new Claim("CartToken", user.CartToken));
            return identity;
        }
    }
}
