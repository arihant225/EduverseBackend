using Csv;
using Eduverse.Backend.Entity.Repository;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Dynamic;
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
        public  IActionResult CompileFile([FromForm] FilePareser file)
        {
            List<ExpandoObject> collection = new();
            SortedList<int,string> columnList=new SortedList<int, string>();
            switch (file.Type.ToUpper())
            {
                case "CSV":
                    using (var stream = file.File.OpenReadStream())
                    {
                        byte[] content = new byte[file.File.Length];
                        stream.Read(content, 0, content.Length);
                        using(var MemoryStream=new MemoryStream(content))
                        { 
                            MemoryStream.Position = 0;  
                            var csvContent = CsvReader.ReadFromStream(MemoryStream);
                            var data=csvContent.ToList();
                            for(int i=0;i<data.Count;i++)
                            {
                                ExpandoObject temp=new ExpandoObject();
                                for(int j = 0; j < data[i].Headers.Count();j++)
                                {
                                    temp.TryAdd(data[i].Headers[j], data[i].Values[j]);
                                }
                                collection.Add(temp);
                                
                            }

                            break;


                        }
                    }
            }
            return StatusCode(200, collection);     
        }




        [HttpPost]

        public async Task<IActionResult> Save(EduverseDirectory? repo) {
            try

            {
                if (repo == null)
                {
                    return BadRequest();
                }
                HttpContext userContext = Request.HttpContext;
                var user = userContext.User;
                Claim? accessor = user?.Claims.Where(ele => ele.Type.ToString().Contains("accessor")).FirstOrDefault();

                if (accessor == null || accessor.Value.Length == 0)
                {
                    userContext.Session?.Clear();
                    return StatusCode(401);
                }
             
                    return StatusCode(200, await this.directoryService.SaveFolder(repo, accessor.Value));
                
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
            Claim? accessor = user?.Claims.Where(ele => ele.Type.ToString().Contains("accessor")).FirstOrDefault();

            if (accessor == null || accessor.Value.Length == 0)
            {
                userContext.Session?.Clear();
                return StatusCode(401);
            }
           
                return StatusCode(200, await this.directoryService.GetDirectory(accessor.Value));
            

        }
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteDirectory(int id) {
            HttpContext userContext = Request.HttpContext;
            var user = userContext.User;
            Claim? accessor = user?.Claims.Where(ele => ele.Type.ToString().Contains("accessor")).FirstOrDefault();

            if (accessor == null || accessor.Value.Length == 0)
            {
                userContext.Session?.Clear();
                return StatusCode(401);
            }
    
              bool success=  await this.directoryService.DeleteFolder(id,accessor.Value);
                if (success)
                    return StatusCode(200, success);
                else
                    return StatusCode(500);


        }
            [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> AllDirectories(int? id)
        {

            HttpContext userContext = Request.HttpContext;
            var user = userContext.User;
            Claim? accessor = user?.Claims.Where(ele => ele.Type.ToString().Contains("accessor")).FirstOrDefault();

            if (accessor == null || accessor.Value.Length == 0)
            {
                userContext.Session?.Clear();
                return StatusCode(401);
            }

            var temp = await this.directoryService.OpenFolder(id, accessor.Value);
            if (temp != null)
                return StatusCode(200, temp);
            return StatusCode(400);
          
        }




    }
}
