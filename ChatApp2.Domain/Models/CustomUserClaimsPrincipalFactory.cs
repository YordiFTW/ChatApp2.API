using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace ChatApp2.Domain.Models
{
    public class CustomUserClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<User>
    {
        public CustomUserClaimsPrincipalFactory(UserManager<User> userManager, IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            //identity.AddClaim(new Claim("locale", user.Locale));
            return identity;
        }
    }
}
