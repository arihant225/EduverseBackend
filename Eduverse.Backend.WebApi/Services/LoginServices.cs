using Eduverse.Backend.Entity.DBModels;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Identity;
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
        public Token? GenerateToken(Login login)
        {
            Credential? cred = this.Repository.getCredentials(login.UserId, login.Password);
            if (cred != null)
            {
                Claim[] claims = new[]
                {
            new Claim("userName", cred.Name),
            new Claim("email", cred.EmailId??""),
            new Claim("phone", "institutionalId", cred.PhoneNumber==null?"0":(cred.PhoneNumber.ToString())),
            new Claim("institutionalId", cred.Institutitional==null?"0":(cred.Institutitional.Guidaccessor??"0")),
            new Claim("accessor", cred.Guidaccessor??""),
        };

                List<Claim> claimsOfanUser = new List<Claim>();
                if (cred.Role!=null&&cred.Role.Any())
                {

                    claimsOfanUser.Add(new Claim("roles", string.Join(",", cred.EduverseRoles.Select(role => role.Role).ToList().ToArray())));
                }
                if (cred.InstitutionalRoles != null && cred.InstitutionalRoles.Any())
                {

                    claimsOfanUser.Add(new Claim("inst-roles", string.Join(",", cred.InstitutionalRoles.Select(role => role.RoleType).ToList().ToArray())));
                }

                claimsOfanUser.AddRange(claims);


                var secretKey = this.configuration["JWT:Secret"];
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
                var issuer = this.configuration["JWT:Issuer"];
                var audience = this.configuration["JWT:Audience"];
                var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(claims: claimsOfanUser, issuer: issuer, expires: DateTime.Now.AddHours(10), signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256), audience: audience);

                return new Token()
                {
                    username = cred.Name,
                    expiration = DateTime.Now.AddMinutes(30),
                    institutionalRoles= cred.InstitutionalRoles.Select(role => string.IsNullOrEmpty(role.RoleType)?"User": role.RoleType).ToList(),
                    JWTToken = new JwtSecurityTokenHandler().WriteToken(token),
                    roles = cred.EduverseRoles.Select(role => role.Role).ToList()
                };
            }
            else
            {
                return null; 
            }
        }

    }
}

