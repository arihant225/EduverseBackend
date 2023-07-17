using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Eduverse.Backend.WebApi.Controllers
{

    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class StreamsController : Controller
    {
        private IEduverseRepository repo;
        public StreamsController(IEduverseRepository repo) {
            this.repo = repo;   
        }
        [HttpGet]
        public IActionResult getAllStream() {

            return StatusCode(200, this.repo.EduverseStreams()?.Select(obj => new Streams() {
                StreamerDescription = obj.StreamerDescription,
                StreamerId = obj.StreamerId,
                StreamerName = obj.StreamerName,
                StreamerType = obj.StreamerType,
                Image = obj.Image,
                Public = obj.Public,
                Paid = obj.Paid == 0 ? false:true,
                Price = obj.Price
            }).ToList()); ;

            
        } 
    }
}
