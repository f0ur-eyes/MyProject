using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace MyProject.Functions
{
    public class Tokens
    {
        private const string secretKey = "abcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyzabcdefghijklmnopqrstuvwxyz";
        private static readonly SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public string generateJwtToken(string id, string email)
        {
            var claims = new[]
            {
                new Claim("userId", id),
                new Claim("email", email)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
                );
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.WriteToken(token);
            return jwt;
        }
        public bool isTokenValid(string token, ref HttpActionContext actionContext)
        {
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;
            if (token == "")
            {
                return false;
            }
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = false
                };
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                return false;
            }
            int userId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            actionContext.Request.Properties["userId"] = userId;
            return true;
        }
    }
}