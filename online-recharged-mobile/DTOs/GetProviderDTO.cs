namespace online_recharged_mobile.DTOs
{
    public class GetProviderDTO
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public double? UserDiscount { get; set; }
        public double? AdminDiscount { get; set; }
        public string? Picture { get; set; }

        public GetProviderDTO() { }

        public GetProviderDTO(Models.ServiceProvider serviceProvider)
        {
            Id = serviceProvider.Id;
            Name = serviceProvider.Name;
            UserDiscount = serviceProvider.UserDiscount;
            AdminDiscount = serviceProvider.AdminDiscount;
            Picture = serviceProvider.Picture;
        }
    }
}
