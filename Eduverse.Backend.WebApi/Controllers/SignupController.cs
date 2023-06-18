using Microsoft.AspNetCore.Mvc;
using ResponseModel=Eduverse.Backend.WebApi.Models.Response;
using RequestModel=Eduverse.Backend.WebApi.Models.Request;
using Microsoft.AspNetCore.Http.HttpResults;
using Eduverse.Backend.Entity.Functionality;
namespace Eduverse.Backend.WebApi.Controllers
{
    [ApiController]
    [Route("/api/[controller]/[action]")]
    public class SignupController : Controller
    {
        [HttpPost]
        public IActionResult generateOtpForMail(RequestModel.Otp otp) {
            if (otp == null)
            {
                BadRequestObjectResult badRequest = BadRequest("Body is Required");
                return badRequest;
            }
            else {
                try
                {
                    OtpGenerator otpGenerator = new();
                    bool isgenerate;
                    int successCode = otpGenerator.generateOtp(otp.Id, otp.UserName, "mail", out isgenerate);
                    ResponseModel.Otp requestModel = new();
                    if (successCode == 1)
                    {
                        requestModel.IsGenerate = isgenerate;
                        requestModel.successCode = 1;
                        requestModel.isAuthenticate = false;

                        return StatusCode(200, requestModel);
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                }
                catch { 
                return StatusCode(500); 
                }
                


            }

            
        
        }
        
    }
}
