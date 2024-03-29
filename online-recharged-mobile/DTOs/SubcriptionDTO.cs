﻿using online_recharged_mobile.Models;

namespace online_recharged_mobile.DTOs
{
    public class SubcriptionDTO
    {
        public long Id { get; set; }
        public double? Value { get; set; }
        public long? ProviderId { get; set; }
        public string? ProviderName { get; set; }
        public double? UserDiscount { get; set; }
        public double? AdminDiscount { get; set; }
        public string? Picture { get; set; }
    }
}
