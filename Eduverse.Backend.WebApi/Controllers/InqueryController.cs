using Eduverse.Backend.Entity.Functionality;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ResponseModel = Eduverse.Backend.WebApi.Models.Response;

namespace Eduverse.Backend.WebApi.Controllers
{

    [AllowAnonymous]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class InqueryController : Controller
    {
        IInqueryService inqueryService;
        public InqueryController(IInqueryService inqueryService) {
            this.inqueryService = inqueryService;
        }
        [HttpPost]
        public async Task<IActionResult> AddInquery([FromForm]Inquery inquery)
        {
            try
            {
                bool added =await inqueryService.AddInquery(inquery);   
                if (added) {
                    return StatusCode(200);
                }
                else
                {
                    return StatusCode(400);

                }

                    }
            catch (Exception ex) {
                return StatusCode(500);
            }    

        }
        [HttpPost]
        public IActionResult GenerateOtpForMail(Otp otp)
        {
            try
            {
                if (otp == null)
                {
                    BadRequestObjectResult badRequest = BadRequest("Body is Required");
                    return badRequest;
                }
                else
                {
                    try
                    {
                        OtpGenerator otpGenerator = new();
                        bool isgenerate;
                        int successCode = otpGenerator.generateOtpForInQuery(otp.Id, otp.UserName, "mail", out isgenerate);
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
                    catch
                    {
                        return StatusCode(500);
                    }



                }
            }
            catch (Exception ex)
            {
                return StatusCode(500);
            }



        }



    }
}
