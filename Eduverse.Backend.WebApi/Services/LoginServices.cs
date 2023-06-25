using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Eduverse.Backend.WebApi.Services
{
    public class LoginServices:ILoginService
    {
        public IEduverseRepository Repository { get; set; }
        public  IConfiguration configuration= new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        public LoginServices(IEduverseRepository repository)
        {
            this.Repository = repository;
            
        }
        public string? GenerateToken(Login login)
        {
            Credential? cred = this.Repository.getCredentials(login.UserId, login.Password);
            if (cred != null)
            {
                Claim[] claims = new[]
                {
            new Claim(ClaimTypes.Name, cred.Name),
            new Claim(ClaimTypes.Email, cred.EmailId),
            new Claim("Phone", cred.PhoneNumber.ToString()),
            new Claim(ClaimTypes.Role, ""+cred.Role)
        };

                var secretKey = this.configuration["JWT:Secret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var issuer = this.configuration["JWT:Issuer"];
                var audience = this.configuration["JWT:Audience"];
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(claims: claims, issuer: issuer, expires: DateTime.Now.AddMinutes(30), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256), audience: audience);
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            else
            {
                return null; 
            }
        }

    }
}


    //ValidIssuer = builder.Configuration["JWT:Issuer"],
    //    ValidAudience = builder.Configuration["JWT:Audience"],
    //    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"])),
    //    ValidateLifetime = true,
    //    ValidateAudience = true,
    //    ValidateIssuer = true

