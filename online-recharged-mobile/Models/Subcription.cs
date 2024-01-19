using System;
using System.Collections.Generic;

namespace online_recharged_mobile.Models
{
    public partial class Subcription
    {
        public Subcription()
        {
            Transactions = new HashSet<Transaction>();
        }

        public long Id { get; set; }
        public double? Value { get; set; }
        public long? ProviderId { get; set; }
        public DateTime? CreateAt { get; set; }
        public string? CreateBy { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual ServiceProvider? Provider { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}
