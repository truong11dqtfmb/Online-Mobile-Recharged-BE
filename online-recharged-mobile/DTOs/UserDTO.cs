namespace online_recharged_mobile.DTOs
{
    public class UserDTO
    {
        public long Id { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime? CreateAt { get; set; }
        public DateTime? ModifyAt { get; set; }
        public bool? IsActive { get; set; }
        public string Email { get; set; } = null!;
        public string? Otp { get; set; }
        public DateTime? VerifyAt { get; set; }
        public string? Picture { get; set; }
        public DateTime? Dob { get; set; }
        public string? Fullname { get; set; }
        public string? Address { get; set; }

        public List<long>? RoleID { get; set; }
    }
}
