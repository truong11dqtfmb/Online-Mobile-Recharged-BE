using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.DTOs;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProviderController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        public ProviderController(rechargedContext rechargedContext, IResponseMessage responseMessage)
        {
            _context= rechargedContext;
            _responseMessage= responseMessage;
        }


        [HttpGet("getallprovider")]
        public async Task<IActionResult> getAllProviders()
        {
            try
            {
                return Ok(_responseMessage.ok("get data successfully", await _context.ServiceProviders
                    .Where(sp => sp.IsActive == true)
                    .Select(sp => new ServiceProviderDTO(sp))
                    .ToListAsync()));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }


        [HttpPost("addprovider")]
        public async Task<IActionResult> addNewProviders(ServiceProviderDTO request)
        {
            try
            {
                var new_provider = new Models.ServiceProvider
                {
                    Name = request.Name,
                    AdminDiscount= request.AdminDiscount,
                    UserDiscount= request.UserDiscount,
                    CreateBy = "Admin1"
                };
                await _context.ServiceProviders.AddAsync(new_provider);
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("add data successfully", new_provider));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
    }
}
