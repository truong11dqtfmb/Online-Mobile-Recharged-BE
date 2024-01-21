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
    [Authorize(Roles = "User")]
    public class UserController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;
        private readonly ICommon _common;

        public UserController(rechargedContext context, IResponseMessage responseMessage, IUserService userService, ICommon common)
        {
            _context = context;
            _responseMessage = responseMessage;
            _userService = userService;
            _common = common;
        }

        [HttpPut("uploadimage")]
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
        public async Task<IActionResult> editUser(EdituserDTO request)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.Username == _userService.getUserName() && u.IsActive == true)
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("data not found");

                user.Fullname = request.Fullname;
                user.Address = request.Address;
                user.Dob = request.Dob;
                user.ModifyAt = DateTime.Now;

                await _context.SaveChangesAsync();
                return Ok(_responseMessage.ok("update data successfully", user));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpPut("changepassword")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDTO changepass)
        {
            try
            {
                var thisuser = await _context.Users
                    .Where(x => x.Username == _userService.getUserName())
                    .SingleOrDefaultAsync();
                if (_common.Hash(changepass.CurrentPassword) == thisuser.Password)
                {
                    thisuser.Password = _common.Hash(changepass.NewPassword);
                    await _context.SaveChangesAsync();
                    return Ok(_responseMessage.ok("Change password sucessfully"));
                }
                return BadRequest(_responseMessage.error("Your current password is incorrect"));

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }
    }
}
