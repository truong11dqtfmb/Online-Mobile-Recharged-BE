using Microsoft.AspNetCore.Connections.Features;

namespace online_recharged_mobile.DTOs
{
    public class PostImageDTO
    {
        public IFormFile? Image { get; set; }
    }
}
