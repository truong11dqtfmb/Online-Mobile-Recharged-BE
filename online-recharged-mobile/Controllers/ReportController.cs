using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.ResponseMessageService;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles ="Admin")]
    public class ReportController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;

        public ReportController(rechargedContext context, IResponseMessage responseMessage)
        {
            _context = context;
            _responseMessage = responseMessage;
        }

        [HttpGet("countserviceprovider")]
        public async Task<IActionResult> countServiceProvider()
        {
            try
            {
                var count = await _context.ServiceProviders
                    .Where(sp => sp.IsActive == true)
                    .CountAsync();
                return Ok(_responseMessage.ok("get data successfully", count));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
        [HttpGet("countuser")]
        public async Task<IActionResult> countUser()
        {
            try
            {
                var count = await _context.Users
                    .Where(u => u.IsActive == true)
                    .CountAsync();
                return Ok(_responseMessage.ok("get data successfully", count));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
        [HttpGet("countTop10user")]
        public async Task<IActionResult> countTop10User()
        {
            try
            {
                var count = await (from t in _context.Transactions
                                    join s in _context.Subcriptions on t.SubcriptionId equals s.ProviderId
                                    group s by new { t.UserId } into g
                                    select new
                                    {
                                        UserId = g.Key.UserId,
                                        Total = g.Sum(x => x.Value)
                                    })
              .OrderByDescending(x => x.Total)
              .Take(10)
              .ToListAsync();
                return Ok(_responseMessage.ok("get data successfully", count));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
        [HttpGet("groupsubcription")]
        public async Task<IActionResult> groupSubcription()
        {
            try
            {
                var count = await (from sp in _context.ServiceProviders
                                   join s in _context.Subcriptions on sp.Id equals s.ProviderId
                                   group s by new { sp.Name, sp.Id} into g
                                   select new
                                   {
                                       Id = g.Key.Id,
                                       Name = g.Key.Name,
                                       Count = g.Count()
                                   })
              .ToListAsync();
                return Ok(_responseMessage.ok("get data successfully", count));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
        
     

        
    }
}
