using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImaginationWorkgroup.Infrastructure.Models.Api
{
    public class ServiceResponse
    {
        public object Model { get; set; }
        public List<ResponseMessage> Messages { get; set; }
        public bool IsError { get; set; }

        public ServiceResponse()
        {

        }

        public ServiceResponse(object model) : this(model, new List<ResponseMessage>())
        {
            
        }

        public ServiceResponse(object model, List<ResponseMessage> messages) : this(model, messages, false)
        {
            
        }

        public ServiceResponse(object model, List<ResponseMessage> messages, bool isError) 
        {
            Model = model;
            Messages = messages;
            IsError = isError;
        }
    }

}
