using System;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Arguments
{
    public class AutenticarResponse
    {
        public string Id { get; set; }
        public string Message { get; set; }
        public UserRole Perfil { get; set;}
        public string Token { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }

        public static explicit operator AutenticarResponse(Usuario v)
        {
            return new AutenticarResponse() { Id = v.Id, Message = "Usuario criado com sucesso" };
        }
    }
}
