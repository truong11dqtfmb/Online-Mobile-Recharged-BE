using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.ResponseMessageService;
using online_recharged_mobile.Services.UserService;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;

        public TransactionController(rechargedContext context, IResponseMessage responseMessage, IUserService userService)
        {
            _context = context;
            _responseMessage = responseMessage;
            _userService = userService;
        }

        [HttpPost("addsubcription")]
        public async Task<IActionResult> addSubcription(int subcriptionid)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == _userService.getUserName())
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("no user found");
                var transaction = new Transaction
                {
                    SubcriptionId = subcriptionid,
                    UserId = user.Id
                };
                await _context.AddAsync(transaction);
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("add data successfully", transaction));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }

        }
    }
}
