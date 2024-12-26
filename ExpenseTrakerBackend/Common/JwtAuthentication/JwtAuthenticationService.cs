using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using PracticeCrud.Model.Login;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PracticeCrud.Common.JwtAuthentication
{
    public class JwtAuthenticationService:IJwtAuthenticationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthenticationService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AccessTokenModel GenerateToken(TokenModel userToken, string JWT_Secret, int JWT_Validity_Mins)
        {
            string serializeToken = JsonConvert.SerializeObject(userToken, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings() { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, serializeToken)
                }),
                Expires = DateTime.UtcNow.AddMinutes(JWT_Validity_Mins),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(JWT_Secret)), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // return basic user info and authentication token
            AccessTokenModel accessTokenVM = new AccessTokenModel();
            accessTokenVM.Token = tokenString;
            accessTokenVM.ValidityInMin = JWT_Validity_Mins;
            accessTokenVM.ExpiresOnUTC = tokenDescriptor.Expires.Value;
            accessTokenVM.UserID = userToken.tokenId;

            return accessTokenVM;
        }

        public TokenModel GetTokenData(string jwtToken)
        {
            TokenModel userTokenData = new TokenModel();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = (JwtSecurityToken)tokenHandler.ReadToken(jwtToken);
            IEnumerable<Claim> claims = securityToken.Claims;
            userTokenData.Token = jwtToken;
            if (claims != null && claims.ToList().Count > 0)
            {
                var claimData = claims.First().Value;
                userTokenData = JsonConvert.DeserializeObject<TokenModel>(claimData) ?? new TokenModel();
                userTokenData.TokenValidTo = securityToken.ValidTo;
            }
            return userTokenData;
        }

        public TokenModel GetTokenDataFromRequest()
        {
            string jwtToken = _httpContextAccessor.HttpContext.Request.Headers[HeaderNames.Authorization].ToString().Replace(JwtBearerDefaults.AuthenticationScheme + " ", "");
            if (!string.IsNullOrEmpty(jwtToken))
            {
                return GetTokenData(jwtToken);
            }
            return null;
        }
    }
}
