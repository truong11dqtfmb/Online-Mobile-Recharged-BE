namespace online_recharged_mobile.DTOs
{
    public class GetTransactionDTO
    {
        public long Id { get; set; }
        public string? Username { get; set; }
        public long? SubcriptionId { get; set; }
        public DateTime? CreateAt { get; set; }
    }
}
