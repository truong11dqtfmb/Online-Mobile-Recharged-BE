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
    public class UserController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;

        public UserController(rechargedContext context, IResponseMessage responseMessage, IUserService userService)
        {
            _context = context;
            _responseMessage = responseMessage;
            _userService = userService;
        }

        [HttpPut("uploadimage")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> postImage([FromForm] PostImageDTO request)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == _userService.getUserName() && u.IsActive == true)
                    .SingleOrDefaultAsync();
                if (request.Image.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", request.Image.FileName);
                    using (var stream = System.IO.File.Create(path))
                    {
                        await request.Image.CopyToAsync(stream);
                    }
                    user.Picture = string.Format("/images/{0}", request.Image.FileName);
                }
                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("add picture successfully"));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpPut("edituser")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> editUser(EdituserDTO request)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == _userService.getUserName() && u.IsActive == true)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found");

                user.Phone= request.Phone;
                user.Email= request.Email;

                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("update data successfully", user));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
    }
}
