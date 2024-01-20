namespace online_recharged_mobile.Services.ResponseMessageService
{
    public class ResponseMessage : IResponseMessage
    {
        public bool? success
        {
            get { return _success; }
            set { _success = value; }
        }

        public string? message
        {
            get { return _message; }
            set { _message = value; }
        }

        public object? data
        {
            get { return _data; }
            set { _data = value; }
        }


        private bool? _success;
        private string? _message;
        private object? _data;



        public ResponseMessage error(string message, object data)
        {
            return new ResponseMessage
            {
                success = false,
                message = message,
                data = data
            };
        }

        public ResponseMessage error(string message)
        {
            return new ResponseMessage
            {
                success = false,
                message = message
            };
        }

        public ResponseMessage ok(string message, object data)
        {
            return new ResponseMessage
            {
                success = true,
                message = message,
                data = data
            };
        }

        public ResponseMessage ok(string message)
        {
            return new ResponseMessage
            {
                success = true,
                message = message,
                data = data
            };
        }
    }
}
