using Eduverse.Backend.Entity.Repository;
using req= Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Net;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Eduverse.Backend.WebApi.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class FileController : Controller
    {
        [HttpPost]
        public IActionResult upload([FromForm] req.File file)
        {

            UploadDownloadService.upload(file.FileItem);
            return StatusCode(200);

        }
        [Route("{path}")]
        [HttpGet]
        public IActionResult getFile(string path) {
            var file= UploadDownloadService.GetDocument(path);
            if(file == null) {

                return StatusCode(400);
            }
            else
            {
                return StatusCode(200, file);

            }
          

            
        }
    }
    
}