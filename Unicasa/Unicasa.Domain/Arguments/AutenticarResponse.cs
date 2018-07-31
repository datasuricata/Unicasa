using System;
using Unicasa.Domain.Entities;

namespace Unicasa.Domain.Arguments
{
    public class AutenticarResponse
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public static explicit operator AutenticarResponse(Usuario v)
        {
            return new AutenticarResponse() { Id = v.Id, Message = "Usuario criado com sucesso" };
        }
    }
}
