namespace online_recharged_mobile.Services.ResponseMessageService
{
    public interface IResponseMessage
    {
        public bool? success { get; set; }
        public string? message { get; set; }
        public object? data { get; set; }

        public ResponseMessage ok(string message, object data);
        public ResponseMessage ok(string message);
        public ResponseMessage error(string message, object data);
        public ResponseMessage error(string message);
    }
}
