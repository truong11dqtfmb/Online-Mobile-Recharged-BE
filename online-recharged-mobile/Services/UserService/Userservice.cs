using System.Security.Claims;

namespace online_recharged_mobile.Services.UserService
{
    public class Userservice : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public Userservice(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public string getUserName()
        {
            if(_httpContextAccessor.HttpContext != null)
            {
                string result = _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Name);
                return result;
            }
            return null;
        }
    }
}
