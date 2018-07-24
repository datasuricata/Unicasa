using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities.Base;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Entities
{
    public class Usuario : BaseEntity
    {
        public Usuario(string email, string senha)
        {
            Email = email;
            Senha = UnicasaExtensions.ConvertToMD5(senha);
        }

        public Usuario()
        {

        }

        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public UserRole UserRole { get; set; }

        public static Usuario Registrar(UsuarioRequest request)
        {
            var usuario = new Usuario()
            {
                Email = request.Email,
                NomeCompleto = request.NomeCompleto,
                Senha = UnicasaExtensions.ConvertToMD5(request.Senha),
                UserRole = request.UserRole
            };

            usuario.GerarId();

            return usuario;
        }
    }
}
