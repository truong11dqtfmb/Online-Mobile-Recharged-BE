﻿using Microsoft.AspNetCore.Http;

namespace online_recharged_mobile.DTOs
{
    public class EdituserDTO
    {
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public DateTime Dob { get; set; }
        public IFormFile? Image { get; set; }
    }
}
