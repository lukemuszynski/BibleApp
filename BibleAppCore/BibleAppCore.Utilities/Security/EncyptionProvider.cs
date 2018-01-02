using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BibleAppApi.Models;
using BibleAppCore.Utilities.ExtensionMethods;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using BibleAppCore.Contracts.Contract.Security;
using Microsoft.IdentityModel.Tokens;

namespace BibleAppCore.Utilities.Security
{

    public class EncyptionProvider : IEncyptionProvider
    {
        private IConfiguration Configuration { get; set; }
        public EncyptionProvider(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public string HashPassword(string password)
        {
            password = password.HashString(Configuration.GetSection("Salt").Value);
            return password;
        }

        public BearerToken CreateBearerToken(BearerToken bearerToken)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Jwt:Key").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(Configuration["Jwt:Issuer"],
                Configuration["Jwt:Issuer"],
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);
            token.Payload.AddClaim(new Claim("login", bearerToken.Login));
            token.Payload.AddClaim(new Claim("guid", bearerToken.Guid.ToString()));
            bearerToken.Token = new JwtSecurityTokenHandler().WriteToken(token);
            return bearerToken;
        }

    }
}
