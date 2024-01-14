using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eduverse.Backend.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthorController : Controller
    {
        public readonly IAuthorService _AuthorService;    
        public AuthorController(IAuthorService authorService)

        {
            _AuthorService = authorService;
            
        }
        [HttpGet]
        public async Task<IActionResult> getStats() {
            return StatusCode(200, await _AuthorService.GetStats());
        }
        [Route("{type}")]
        [HttpGet]
        public async Task<IActionResult> SearchInstitute([FromRoute] string type)
        {
            try
            {
                dynamic listOfInstitute=await _AuthorService.SearchInstitute(type);
                if(listOfInstitute==null)
                {
                    return BadRequest();
                }
                else
                {
                    return Ok(listOfInstitute); 
                }
            }
            catch
            {
                return StatusCode(500);
            }
    }

        [HttpPost]
        public async Task<IActionResult> ManageInstitues(InstituteActions actions)
        {
            await this._AuthorService.ManageInstitutes(actions);
            return Ok(actions);    
        }
    }
   
}
