using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eduverse.Backend.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DirectoryController : Controller
    {
        
       private readonly IDirectoryService directoryService;
       public DirectoryController(IDirectoryService directoryService) {
        this.directoryService = directoryService;
        }


        [HttpPost]

        public async Task<IActionResult> Save(EduverseDirectory? repo) {
            try

            {
                if(repo == null)
                {
                    return BadRequest();
                }
                HttpContext userContext = Request.HttpContext;
                var user = userContext.User;
                Claim? emailClaim = user?.Claims.Where(ele => ele.Type.ToString().Contains("emailaddress")).FirstOrDefault();
                string? email = emailClaim?.Value;
                Claim? phoneClaim = user?.Claims.Where(ele => ele.Type == "phone").FirstOrDefault();
                decimal? Phoneno = Convert.ToDecimal(phoneClaim?.Value);
                if (email != null)
                return StatusCode(200, await this.directoryService.SaveFolder(repo, email, Phoneno));
                return StatusCode(401);
            }
            catch {
                return StatusCode(500);
            }
        }
        [HttpGet]
        public async Task<IActionResult> AllDirectories() 
        {
            HttpContext userContext = Request.HttpContext;
            var user = userContext.User;
            Claim? emailClaim = user?.Claims.Where(ele => ele.Type.ToString().Contains("emailaddress")).FirstOrDefault();
            string? email = emailClaim?.Value;
            Claim? phoneClaim = user?.Claims.Where(ele => ele.Type == "phone").FirstOrDefault();
            decimal? Phoneno = Convert.ToDecimal(phoneClaim?.Value);
            if(email!= null)    
            return StatusCode(200, await this.directoryService.GetDirectory(email, Phoneno));
            return StatusCode(401);

        }



    }
}
