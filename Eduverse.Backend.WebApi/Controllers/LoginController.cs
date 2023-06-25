using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Eduverse.Backend.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : Controller
    {
        public readonly ILoginService loginService;
        public LoginController(ILoginService loginService)
        {
            this.loginService = loginService;

        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Authenticate(Login login) {
            string? token=this.loginService.GenerateToken(login);
            if (token != null)
            {
                return StatusCode(200, token);
            }
            else {
                return StatusCode(401, null);
            }
            
        
        }



        [HttpGet]
        [Authorize]
        public IActionResult IsAuthorized() {
            return StatusCode(200,true);

        }
    }
}
