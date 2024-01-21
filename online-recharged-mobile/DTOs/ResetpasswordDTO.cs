using System.ComponentModel.DataAnnotations;

namespace online_recharged_mobile.DTOs
{
    public class ResetpasswordDTO
    {
        [EmailAddress]
        public string Email { get; set; }
    }
}
