using System;
using System.Collections.Generic;

namespace online_recharged_mobile.Models
{
    public partial class ServiceProvider
    {
        public ServiceProvider()
        {
            Subcriptions = new HashSet<Subcription>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public double? UserDiscount { get; set; }
        public double? AdminDiscount { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<Subcription> Subcriptions { get; set; }
    }
}
