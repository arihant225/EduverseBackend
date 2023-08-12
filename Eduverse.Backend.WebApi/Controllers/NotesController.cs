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
    public class NotesController : Controller
    {
        private readonly INotesService _notesService;
        public NotesController(INotesService notesService) {
            _notesService = notesService;    
        }

        [HttpPost]
        public async Task<IActionResult> SaveNotes(Notes notes) {
            try
            {

                HttpContext userContext = Request.HttpContext;
                var user= userContext.User;
                Claim? emailClaim = user?.Claims.Where(ele => ele.Type.ToString().Contains("emailaddress")).FirstOrDefault();
                string? email=emailClaim?.Value;
                Claim? phoneClaim = user?.Claims.Where(ele => ele.Type == "phone").FirstOrDefault();
                decimal?  Phoneno= Convert.ToDecimal(phoneClaim?.Value);
                if (email == null)
                {
                    userContext.Session?.Clear();
                    return StatusCode(401);
                }
                if (notes == null)
                {
                    return StatusCode(500);
                }
                 return StatusCode(200,await this._notesService.SaveNotes(email, Phoneno.GetValueOrDefault(), notes));



            }
            catch {

                return StatusCode(500);
            }
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetNotes(long id)
        {
            try
            {

                HttpContext userContext = Request.HttpContext;
                var user = userContext.User;
                Claim? emailClaim = user?.Claims.Where(ele => ele.Type.ToString().Contains("emailaddress")).FirstOrDefault();
                string? email = emailClaim?.Value;
                Claim? phoneClaim = user?.Claims.Where(ele => ele.Type == "phone").FirstOrDefault();
                decimal? Phoneno = Convert.ToDecimal(phoneClaim?.Value);
                if (email == null)
                {
                    userContext.Session?.Clear();
                    return StatusCode(401);
                }
                if (id == 0)
                {
                    return StatusCode(404);
                }
                var note = await this._notesService.getNotesById(id, email, Phoneno.GetValueOrDefault());
                if (note == null)
                {
                    return StatusCode(404);
                }
                return StatusCode(200,note);



            }
            catch
            {

                return StatusCode(500);
            }

        }
    }
}
