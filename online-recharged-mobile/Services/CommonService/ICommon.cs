using online_recharged_mobile.Models;

namespace online_recharged_mobile.Services.CommonService
{
    public interface ICommon
    {
        string createToken(User user);
        string Hash(string value);
        void sendEmail(string subject, string body, string to);
        string CreateRandomString(int length);
    }
}
