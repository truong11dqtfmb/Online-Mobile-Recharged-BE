using online_recharged_mobile.Models;

namespace online_recharged_mobile.DTOs
{
    public class ServiceProviderDTO
    {
        public string? Name { get; set; }
        public double? UserDiscount { get; set; }
        public double? AdminDiscount { get; set; }

        public ServiceProviderDTO() { }

        public ServiceProviderDTO(Models.ServiceProvider serviceProvider)
        {
            Name = serviceProvider.Name;
            UserDiscount = serviceProvider.UserDiscount;
            AdminDiscount = serviceProvider.AdminDiscount;
        }

    }
}
