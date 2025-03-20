namespace ImaginationWorkgroup.Infrastructure.Models.Api
{
    public class ResponseMessage
    {
        public string Message { get; set; }
        //todo: options for severity
        public ResponseMessage()
        {

        }

        public ResponseMessage(string message)
        {
            Message = message;
        }
    }

}
