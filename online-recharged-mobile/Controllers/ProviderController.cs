using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.DTOs;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.ResponseMessageService;
using online_recharged_mobile.Services.UserService;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ProviderController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;
        public ProviderController(rechargedContext rechargedContext, IResponseMessage responseMessage, IUserService userService)
        {
            _context = rechargedContext;
            _responseMessage = responseMessage;
            _userService = userService;
        }


        [HttpGet("getallprovider")]
        public async Task<IActionResult> getAllProviders()
        {
            try
            {
                return Ok(_responseMessage.ok("get data successfully", await _context.ServiceProviders
                    .Where(sp => sp.IsActive == true)
                    .Select(sp => new GetProviderDTO(sp))
                    .ToListAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


        [HttpGet("getproviderbyid/{id}")]
        public async Task<IActionResult> getProviderByID(int id)
        {
            try
            {
                return Ok(_responseMessage.ok("get data successfully", await _context.ServiceProviders
                    .Where(sp => sp.IsActive == true && sp.Id == id)
                    .Select(sp => new GetProviderDTO(sp))
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found")));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


        [HttpPost("addprovider")]
        public async Task<IActionResult> addNewProviders([FromForm] ServiceProviderDTO request)
        {
            try
            {
                var new_provider = new Models.ServiceProvider
                {
                    Name = request.Name,
                    AdminDiscount = request.AdminDiscount,
                    UserDiscount = request.UserDiscount,
                    CreateBy = _userService.getUserName()
                };
                if (request.Image.Length > 0)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{timestamp}_{request.Image.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "providers", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await request.Image.CopyToAsync(stream);
                    }
                    new_provider.Picture = string.Format("https://localhost:7067/images/providers/{0}", fileName);
                }
                await _context.ServiceProviders.AddAsync(new_provider);
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("add data successfully", new_provider));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
        [HttpPut("editprovider/{id}")]
        public async Task<IActionResult> editProviders([FromForm] ServiceProviderDTO request, int id)
        {
            try
            {
                var provider = await _context.ServiceProviders
                    .Where(sp => sp.Id == id && sp.IsActive == true)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found");

                provider.Name = request.Name;
                provider.AdminDiscount = request.AdminDiscount;
                provider.UserDiscount = request.UserDiscount;
                provider.ModifyAt = DateTime.Now;
                provider.ModifyBy = _userService.getUserName();
                if (request.Image.Length > 0)
                {
                    string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                    string fileName = $"{timestamp}_{request.Image.FileName}";
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "providers", fileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await request.Image.CopyToAsync(stream);
                    }
                    provider.Picture = string.Format("https://localhost:7067/images/providers/{0}", fileName);
                }

                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("update data successfully", provider));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
        [HttpPut("deleteprovider/{id}")]
        public async Task<IActionResult> deleteProviders(int id)
        {
            try
            {
                var provider = await _context.ServiceProviders
                    .Where(sp => sp.Id == id && sp.IsActive == true)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found");

                provider.IsActive = false;
         
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("delete data successfully", provider));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
    }
}
