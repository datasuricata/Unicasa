namespace Unicasa.Domain.Arguments.Base
{
    public class BaseResponse
    {
        public BaseResponse()
        {
            Message = "Operação Executada";
        }

        public string Id { get; set; }

        public string Message { get; set; }
    }
}
