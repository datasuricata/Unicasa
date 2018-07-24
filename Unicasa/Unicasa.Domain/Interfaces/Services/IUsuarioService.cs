using System.Collections.Generic;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Services.Base;

namespace Unicasa.Domain.Interfaces.Services
{
    public interface IUsuarioService : IBaseService
    {
        AutenticarResponse AutenticarUsuario(AutenticarRequest request);
        void AdicionarUsuario(UsuarioRequest request);
        IEnumerable<Usuario> ListarUsuario();
    }
}
