using PracticeCrud.Model.Login;

namespace PracticeCrud.Common.JwtAuthentication
{
    public interface IJwtAuthenticationService
    {
        AccessTokenModel GenerateToken(TokenModel userToken, string JWT_Secret, int JWT_Validity_Mins);
        TokenModel GetTokenData(string jwtToken);
        TokenModel GetTokenDataFromRequest();
    }
}
