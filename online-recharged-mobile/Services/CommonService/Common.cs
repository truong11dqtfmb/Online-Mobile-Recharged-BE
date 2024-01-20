using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using online_recharged_mobile.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace online_recharged_mobile.Services.CommonService
{
    public class Common : ICommon
    {
        private readonly rechargedContext _context;
        private readonly IConfiguration _configuration;

        public Common(rechargedContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public string CreateRandomString(int length)
        {
            Random RNG = new Random();
            var rString = "";
            for (var i = 0; i < length; i++)
            {
                rString += ((char)(RNG.Next(1, 26) + 64)).ToString().ToLower();
            }
            return rString;
        }

        public string createToken(User user)
        {
            
            var userrole_name = _context.UserRoles
                .Where(ur => ur.UserId == user.Id)
                .Select(ur => ur.Role)
                .Select(r => r.Name)
                .ToList();

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, user.Username),
            };
            userrole_name.ForEach(urn =>
            {
                claims.Add(new Claim(ClaimTypes.Role, urn));

            });
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Key").Value));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken
            (
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred
            );
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public string Hash(string value)
        {
            using (SHA512 shaM = new SHA512Managed())
            {
                byte[] bytes = shaM.ComputeHash(Encoding.UTF8.GetBytes(value));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public void sendEmail(string subject, string body, string to)
        {
            string fromMail = "onlinerechargedmobile@gmail.com";
            string fromPassword = "rfmhlblzfrvhrdbd";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(to));
            message.Body = body;
            message.IsBodyHtml = false;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true
            };
            smtpClient.Send(message);
        }

    }
}
