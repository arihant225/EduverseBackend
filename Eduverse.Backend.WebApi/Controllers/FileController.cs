using Eduverse.Backend.Entity.Repository;
using req= Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Eduverse.Backend.WebApi.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : Controller
    {
        [HttpPost]
        public IActionResult upload([FromForm] req.File file)
        {
            UploadDownloadService uploadDownloadService = new UploadDownloadService();

            uploadDownloadService.upload(file.FileItem);
            return StatusCode(200);

        }

    }
}