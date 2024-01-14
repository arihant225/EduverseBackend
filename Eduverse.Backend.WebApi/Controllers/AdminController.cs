using Eduverse.Backend.WebApi.Models.Response;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;

namespace Eduverse.Backend.WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AdminController : Controller
    {
        IAdminService adminService;
        public AdminController(IAdminService adminService) {
            this.adminService = adminService;

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Domains(int? id)
        {
            string? accessor = GetAccessor();
            if (id == 0)
                id = null;
            var data = await this.adminService.GetAllDomains(id, accessor);
            if (data != null)
            {
                return Ok(data);
            }
            return BadRequest();
        }
        [HttpPost]
        public async Task<IActionResult> Domains(Domain domain)
        {
            Domain? data = null;
            string? accessor = GetAccessor();
             if(accessor!=null)
             data = await this.adminService.UpdateOrAddDomain(domain, accessor);
            if (data!=null)
            {
                return Ok(data);
            }
            return BadRequest();
        }

        private string? GetAccessor()
        {
            HttpContext userContext = Request.HttpContext;
            var user = userContext.User;
            Claim? accessor = user?.Claims.Where(ele => ele.Type.ToString().Contains("accessor")).FirstOrDefault();

            if (accessor == null || accessor.Value.Length == 0)
            {
                userContext.Session?.Clear();
               
            }
            return accessor?.Value; 

        }

    }
}
