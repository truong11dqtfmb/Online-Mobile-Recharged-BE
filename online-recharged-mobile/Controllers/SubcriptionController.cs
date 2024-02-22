using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using online_recharged_mobile.DTOs;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.ResponseMessageService;
using online_recharged_mobile.Services.UserService;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class SubcriptionController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;

        public SubcriptionController(rechargedContext rechargedContext, IResponseMessage responseMessage, IUserService userService)
        {
            _context = rechargedContext;
            _responseMessage = responseMessage;
            _userService = userService;
        }

        [HttpGet("getallsubcription")]
        [AllowAnonymous]
        public async Task<IActionResult> getAllSubcription()
        {
            try
            {
                return Ok(_responseMessage.ok("get data successfully", (await _context.Subcriptions
                    .Where(s => s.IsActive == true)
                    .Select(s => new SubcriptionDTO
                    {
                        Value = s.Value,
                        ProviderName = s.Provider.Name
                    })
                    .ToListAsync())));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpGet("getsubcriptionbyid/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> getSubcriptionById(int id)
        {
            try
            {
                return Ok(_responseMessage.ok("get data successfully", (await _context.Subcriptions
                    .Where(s => s.IsActive == true && s.Id == id)
                    .Select(s => new SubcriptionDTO
                    {
                        Value = s.Value,
                        ProviderName = s.Provider.Name
                    })
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found"))));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpPost("addnewsubcription")]
        public async Task<IActionResult> addNewSubcription(AddSubcriptionDTO request)
        {
            try
            {
                if (request != null)
                {
                    var new_subcription = new Subcription
                    {
                        Value = request.Value,
                        ProviderId = request.ProviderId
                    };
                    await _context.Subcriptions.AddAsync(new_subcription);
                    await _context.SaveChangesAsync();
                    return Ok(_responseMessage.ok("add data successfully", new_subcription));
                }
                return BadRequest(_responseMessage.error("data must not be empty"));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


        [HttpPut("editsubcription/{id}")]
        public async Task<IActionResult> editSubcription(AddSubcriptionDTO request, int id)
        {
            try
            {
                if (request != null)
                {
                    var subcription = await _context.Subcriptions
                        .Where(s => s.Id == id && s.IsActive == true)
                        .SingleOrDefaultAsync();
                    subcription.Value = request.Value;
                    subcription.ProviderId = request.ProviderId;
                    await _context.SaveChangesAsync();
                    return Ok(_responseMessage.ok("edit data successfully", subcription));
                }
                return BadRequest(_responseMessage.error("data must not be empty"));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


        [HttpPut("deletesubcription/{id}")]
        public async Task<IActionResult> deleteSubcription(int id)
        {
            try
            {

                var subcription = await _context.Subcriptions
                    .Where(s => s.Id == id && s.IsActive == true)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found");
                subcription.IsActive = false;
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("delete data successfully"));


            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


    }
}
