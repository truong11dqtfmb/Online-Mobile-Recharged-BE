using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using online_recharged_mobile.DTOs;
using online_recharged_mobile.Models;
using online_recharged_mobile.Services.CommonService;
using online_recharged_mobile.Services.ResponseMessageService;
using online_recharged_mobile.Services.UserService;
using System.Reflection.Metadata.Ecma335;

namespace online_recharged_mobile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly rechargedContext _context;
        private readonly IResponseMessage _responseMessage;
        private readonly IUserService _userService;
        private readonly ICommon _common;

        public AuthController(rechargedContext context, IResponseMessage responseMessage, ICommon common, IUserService userService)
        {
            _context = context;
            _responseMessage = responseMessage;
            _common = common;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(LoginDTO request)
        {
            try
            {
                if (request != null)
                {
                    var user = await _context.Users
                        .Where(u => u.Username == request.Username && _common.Hash(request.Password) == u.Password && u.IsActive == true)
                        .SingleOrDefaultAsync();
                    if (user != null)
                    {
                        string token = _common.createToken(user);
                        return Ok(_responseMessage.ok("login successfully", token));
                    }
                    else
                    {
                        return BadRequest(_responseMessage.error("username or password not valid"));
                    }
                }
                else
                {
                    return BadRequest(_responseMessage.error("username and password cannot be empty"));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO request)
        {
            try
            {
                if (request.Password != request.Confirmpassword)
                {
                    return BadRequest(_responseMessage.error("Password does not match"));
                }
                var checkuser = await _context.Users
                            .Where(x => (x.Email == request.Email || x.Username == request.Username) && x.IsActive == true)
                            .SingleOrDefaultAsync();

                if (checkuser == null)
                {
                    string OTP;
                    while (true)
                    {
                        OTP = _common.CreateRandomString(6).ToUpper();
                        if (!_context.Users.Any(x => x.Otp == OTP))
                        {
                            break;
                        }
                    }

                    var newuser = new User
                    {
                        Username = request.Username,
                        Email = request.Email,
                        Password = _common.Hash(request.Password),
                        Otp = OTP,
                        Phone = request.Phone,
                        Dob = request.Dob,
                        IsActive = false,
                        Address = request.Address,
                        Fullname= request.Fullname,
                    };

                    await _context.Users.AddAsync(newuser);
                    await _context.SaveChangesAsync();
                    await _context.UserRoles.AddAsync(new UserRole
                    {
                        RoleId = 2,
                        UserId = newuser.Id,
                    });
                    await _context.SaveChangesAsync();
                    string body = string.Format("Your code verify is: {0}", newuser.Otp);
                    _common.sendEmail("Verifry Email", body, newuser.Email);

                    return Ok(_responseMessage.ok("Please verify your email"));
                }
                else
                {
                    return BadRequest(_responseMessage.error("Email or username has already existed"));
                }

            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error((ex.InnerException != null) ? ex.InnerException.Message : ""));
            }
        }

        [HttpPut("verify")]
        public async Task<IActionResult> Verify(VerifyDTO request)
        {
            try
            {
                var unverified_user = await _context.Users
                .Where(u => u.Otp == request.Otp.ToUpper())
                .SingleOrDefaultAsync();
                if (unverified_user != null)
                {
                    if (unverified_user.IsActive == false)
                    {
                        unverified_user.VerifyAt = DateTime.Now;
                        unverified_user.IsActive = true;
                        await _context.SaveChangesAsync();
                        return Ok(_responseMessage.ok("Create account sucessfully ", unverified_user));
                    }
                    else
                    {
                        return BadRequest(_responseMessage.error("User had already verified"));
                    }
                }
                return BadRequest(_responseMessage.error("Invalid code"));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
            
        }


        [HttpPut("resestpassword")]
        public async Task<IActionResult> ResetPassword(ResetpasswordDTO request)
        {
            try
            {
                var user = await _context.Users
               .Where(u => u.Email == request.Email && u.IsActive == true)
               .SingleOrDefaultAsync();
                if (user != null)
                {
                    string resetpassword = _common.CreateRandomString(10);
                    user.Password = _common.Hash(resetpassword);
                    await _context.SaveChangesAsync();
                    string body = "Your Password is: " + resetpassword;
                    _common.sendEmail("Reset Password", body, request.Email);
                    return Ok(_responseMessage.ok("We've sent new password to your email, please check your email"));
                }
                return BadRequest(_responseMessage.error("Your email hasn't been used"));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
           
        }

        [HttpGet("getcurrentuser")]
        [Authorize(Roles ="User")]
        public async Task<IActionResult> GetCurrent()
        {
            try
            {
                var username = _userService.getUserName();
                var user = await _context.Users
                    .Where(u => u.Username == username && u.IsActive == true)
                    .Select(u => new UserDTO
                    {
                        Email = u.Email,
                        IsActive = u.IsActive,
                        Address= u.Address,
                        CreateAt= u.CreateAt,
                        Dob = u.Dob,
                        Fullname= u.Fullname,
                        Id= u.Id,
                        ModifyAt= u.ModifyAt,
                        Otp = u.Otp,
                        Password = u.Password,
                        Phone= u.Phone,
                        Picture= u.Picture,
                        Username= username,
                        VerifyAt= u.VerifyAt,
                        RoleID = u.UserRoles
                        .Select(ur => ur.Role)
                        .Select(r => r.Id)
                        .ToList()
                    })
                    .SingleOrDefaultAsync()
                    ?? throw new Exception("No user found");
                return Ok(_responseMessage.ok("Get succesfully", user));
            }
            catch (Exception ex)
            {
                return BadRequest(_responseMessage.error(ex.Message));
            }
            
        }
    }
}
