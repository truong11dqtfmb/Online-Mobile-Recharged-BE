using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.DTOs;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.CommonService;
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
        private readonly ICommon _common;

        public TransactionController(rechargedContext context, IResponseMessage responseMessage, IUserService userService, ICommon common)
        {
            _context = context;
            _responseMessage = responseMessage;
            _userService = userService;
            _common = common;
        }

        [HttpPost("addsubcription")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> addSubcription(AddTransactionDTO request)
        {
            try
            {
                var userID = await _context.Users
                    .Where(u => u.Username == _userService.getUserName())
                    .Select(u => u.Id)
                    .SingleOrDefaultAsync();
                var transaction = new Transaction
                {
                    SubcriptionId = request.SubcriptionId,
                    UserId = userID
                };
                await _context.Transactions.AddAsync(transaction);
                await _context.SaveChangesAsync();
                var subcriptionValue = await _context.Subcriptions
                    .Where(s => s.Id == request.SubcriptionId && s.IsActive == true)
                    .Select(s => s.Value)
                    .SingleOrDefaultAsync();
                var provider = await _context.Subcriptions
                    .Where(s => s.Id == request.SubcriptionId && s.IsActive == true)
                    .Select(s => s.Provider)
                    .Select(p => p.Name) 
                    .SingleOrDefaultAsync();
                var subject = "Successfully charged";
                var body = String.Format("You've successfully charged {0} VND from {1} provider", subcriptionValue, provider);
                var userEmail = await _context.Users
                    .Where(u => u.Username == _userService.getUserName())
                    .Select(u => u.Email)
                    .SingleOrDefaultAsync();
                _common.sendEmail(subject, body, userEmail);
                return Ok(_responseMessage.ok("add data successfully", transaction));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }

        }
        
        [HttpGet("getalltransactions")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetAllTransactions()
        {
            try
            {
                var lstTransaction = await _context.Transactions
                    .Where(t => t.IsActive == true)
                    .Select(t => new GetTransactionDTO
                    {
                        Id= t.Id,
                        CreateAt= t.CreateAt,
                        Username = t.User.Username,
                        SubcriptionId= t.SubcriptionId,
                    })
                    .ToListAsync();
                return Ok(_responseMessage.ok("Get data successfully", lstTransaction));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }

        }
        
        [HttpGet("gettransactionsbyID/{id}")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> GetTransactionsByID(long id)
        {
            try
            {
                var lstTransaction = await _context.Transactions
                    .Where(t => t.Id == id && t.IsActive == true)
                    .Select(t => new GetTransactionDTO
                    {
                        Id= t.Id,
                        CreateAt= t.CreateAt,
                        Username = t.User.Username,
                        SubcriptionId= t.SubcriptionId,
                    })
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("No data found");
                return Ok(_responseMessage.ok("Get data successfully", lstTransaction));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }

        }

    }
}
