using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Arguments
{
    public class UsuarioRequest
    {
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public UserRole UserRole { get; set; }
    }
}
