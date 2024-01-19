using System;
using System.Collections.Generic;

namespace online_recharged_mobile.Models
{
    public partial class Transaction
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public long? SubcriptionId { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public string? ModifyBy { get; set; }
        public bool? IsActive { get; set; }

        public virtual Subcription? Subcription { get; set; }
        public virtual User? User { get; set; }
    }
}
