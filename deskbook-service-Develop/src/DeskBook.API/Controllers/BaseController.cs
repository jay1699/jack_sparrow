using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace DeskBook.API.Controllers
{
    public class BaseController : ControllerBase
    {
        public string Token
        {
            get
            {
                string authorizationHeader = HttpContext.Request.Headers["Authorization"];
                string accessToken = string.Empty;

                if (!string.IsNullOrEmpty(authorizationHeader) && authorizationHeader.StartsWith("Bearer "))
                {
                    accessToken = authorizationHeader.Substring("Bearer ".Length);
                }
                return accessToken;
            }
        }

        private string GetClaim(string claimName)
        {
            
            var role = string.Empty;
            try
            {
                if (!string.IsNullOrEmpty(Token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var token = handler.ReadToken(Token) as JwtSecurityToken;
                     role = token?.Claims.First(x => x.Type == claimName).Value;
                }
            }
            catch
            {
                throw;
            }
            return role;
        }

        protected string UserRegisteredId
        {
            get
            {
                return GetClaim("sub");
            }
        }

        protected int UserId
        {
            get
            {
                var userId = GetClaim("userId");
                return int.Parse(userId);
            }
        }
    }
}