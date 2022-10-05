using System.IdentityModel.Tokens.Jwt;

namespace WebApi.Services
{
    public class GetInfoFromToken
    {
        public Dictionary<string, string> GetInfoFromTokenLogin(string token)
        {
            var TokenInfo = new Dictionary<string, string>();
            var handler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = handler.ReadJwtToken(token);
            var claims = jwtSecurityToken.Claims.ToList();
            foreach (var claim in claims)
            {
                TokenInfo.Add(claim.Type, claim.Value);
            }
            return TokenInfo;
        }
    }
}