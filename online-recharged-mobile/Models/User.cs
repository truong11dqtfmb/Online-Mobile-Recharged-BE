using System;
using System.Collections.Generic;

namespace online_recharged_mobile.Models
{
    public partial class User
    {
        public User()
        {
            Feedbacks = new HashSet<Feedback>();
            Transactions = new HashSet<Transaction>();
            UserRoles = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Phone { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public bool? IsActive { get; set; }
        public string? Email { get; set; }

        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
