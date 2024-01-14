using Eduverse.Backend.Entity.Functionality;
using Eduverse.Backend.WebApi.Models.Request;
using Eduverse.Backend.WebApi.Services.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<IActionResult> AddInquery([FromForm] Inquery inquery)
        {
            try
            {
                string? accessor = await inqueryService.AddInquery(inquery);
                if (accessor != null) {
                    return StatusCode(200, Json(accessor));
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
        [Route("{proposal}")]
        [HttpGet]
        public async Task<IActionResult> SearchProposal(string proposal)
        {
            var inquery = await this.inqueryService.SearchProposal(proposal);
            if (inquery == null)
                return BadRequest();
            else
            { return StatusCode(200, inquery);

            }


        }
        [Route("{proposal}")]
        [HttpPost]
        public async Task<IActionResult> GenerateOtpForWithdrawProposal([FromRoute] string proposal, [FromBody] Otp otp)
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
                        bool valid = await this.inqueryService.validProposal(proposal, otp.Id);
                        if (!valid)
                        {
                            return BadRequest();
                        }
                        OtpGenerator otpGenerator = new();
                        bool isgenerate;
                        int successCode = otpGenerator.generateOtpForWithdrawingProposal(otp.Id, otp.UserName, "mail", out isgenerate);
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

        [Route("{proposal}")]
        [HttpPost]
        public async Task<IActionResult> WithdrawProposal([FromRoute] string proposal)
        {
            bool status= await this.inqueryService.ModifyProposal(InstitutionalStatus.Withdrawn,proposal);
            if (!status)
                return BadRequest();    
            return Ok(status);
                
        }


    }
}
