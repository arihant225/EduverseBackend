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
    }
}
