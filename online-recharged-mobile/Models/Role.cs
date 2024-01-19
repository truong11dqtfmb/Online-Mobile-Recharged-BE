using System;
using System.Collections.Generic;

namespace online_recharged_mobile.Models
{
    public partial class Role
    {
        public Role()
        {
            UserRoles = new HashSet<UserRole>();
        }

        public long Id { get; set; }
        public string? Name { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public bool? IsActive { get; set; }

        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}
