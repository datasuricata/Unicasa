using System.Collections.Generic;
using System.Text;

namespace Unicasa.Domain.Arguments.Base
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Message = "Operação não executada";
            Exceptions = new List<string>();
        }

        public string Id { get; set; }
        public string Message { get; set; }
        public List<string> Exceptions { get; set; }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            Exceptions.ForEach(x =>{builder.AppendLine(x);});
            return builder.ToString();
        }
    }
}
