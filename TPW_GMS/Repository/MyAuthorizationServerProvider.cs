using Microsoft.Owin.Security.OAuth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using TPW_GMS.Data;

namespace TPW_GMS.Repository
{
    public class MyAuthorizationServerProvider : OAuthAuthorizationServerProvider
    {
        public override async Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            context.Validated();
        }
        public override async Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            using (UserMasterRepository _repo = new UserMasterRepository())
            {
                double number;
                if (Double.TryParse(context.UserName, out number))
                {
                   MemberInformation user = _repo.ValidateStaff(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Role, "1" ));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.username));
                    context.Validated(identity);
                }
                else
                {
                   Login user = _repo.ValidateUser(context.UserName, context.Password);
                    if (user == null)
                    {
                        context.SetError("invalid_grant", "Provided username and password is incorrect");
                        return;
                    }
                    var identity = new ClaimsIdentity(context.Options.AuthenticationType);
                    identity.AddClaim(new Claim(ClaimTypes.Role, user.roleId.ToString()));
                    identity.AddClaim(new Claim(ClaimTypes.Name, user.username));
                    context.Validated(identity);
                }
                    
                
               
            }
        }
    }
}