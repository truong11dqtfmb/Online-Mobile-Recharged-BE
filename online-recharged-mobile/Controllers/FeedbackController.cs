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
    public class FeedbackController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;

        public FeedbackController(rechargedContext context, IResponseMessage responseMessage, IUserService userService)
        {
            _context = context;
            _responseMessage = responseMessage;
            _userService = userService;
        }

        [HttpGet("getallfeedback")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllFeedback()
        {
            try
            {
                var lstFeedback = await _context.Feedbacks
                    .Where(f => f.IsActive == true)
                    .Select(f => new GetFeedbackDTO
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Username = f.User.Username
                    })
                    .ToListAsync();
                return Ok(_responseMessage.ok("Get data successfully", lstFeedback));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpGet("getfeedbackbyID/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> getAllFeedback(long id)
        {
            try
            {
                var feedback = await _context.Feedbacks
                    .Where(f => f.IsActive == true && f.Id == id)
                    .Select(f => new GetFeedbackDTO
                    {
                        Id = f.Id,
                        Content = f.Content,
                        Username = f.User.Username
                    })
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("No data found");
                return Ok(_responseMessage.ok("Get data successfully", feedback));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpPost("givefeedback")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GiveFeedback(GiveFeedbackDTO request)
        {
            try
            {
                var userID = await _context.Users
                    .Where(u => u.Username == _userService.getUserName() && u.IsActive == true)
                    .Select(u => u.Id)
                    .SingleOrDefaultAsync();
                if (request != null)
                {
                    var newFeedback = new Feedback
                    {
                        Content = request.Content,
                        UserId = userID
                    };
                    await _context.Feedbacks.AddAsync(newFeedback);
                    await _context.SaveChangesAsync();
                    return Ok(_responseMessage.ok("Add data successfully", newFeedback));
                }
                return BadRequest(_responseMessage.error("Content must not be empty"));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
    }
}
