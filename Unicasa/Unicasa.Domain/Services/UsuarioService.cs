using System.Collections.Generic;
using System.Linq;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;
using Unicasa.Domain.Interfaces.Services;
using Unicasa.Domain.Interfaces.Services.Base;

namespace Unicasa.Domain.Services
{
    public class UsuarioService : IBaseService, IUsuarioService
    {
        List<string> Notification = new List<string>();

        private readonly IUsuarioRepository usuarioRepository;

        public UsuarioService(IUsuarioRepository usuarioRepository)
        {
            this.usuarioRepository = usuarioRepository;
        }

        public void AdicionarUsuario(UsuarioRequest request)
        {
            var usuario = Usuario.Registrar(request);

            if (usuarioRepository.Existe(x => x.Email == request.Email))
                Notification.Add("Já existe um usuário com o e-mail informado");

            usuario = usuarioRepository.Adicionar(usuario);
        }

        public AutenticarResponse AutenticarUsuario(AutenticarRequest request)
        {
            if (request == null)
                Notification.Add("Erro o request esta sem conteúdo, entre em contato com o desenvolvedor");

            var usuario = new Usuario(request.Email, request.Senha);

            usuario = usuarioRepository.ObterPor(x => x.Email == usuario.Email && x.Senha == usuario.Senha);

            if (usuario == null)
                return new AutenticarResponse() { Id = string.Empty, Message = "Erro ao autenticar o usuario, UsuarioService" };

            return (AutenticarResponse)usuario;
        }

        public IEnumerable<Usuario> ListarUsuario()
        {
            return usuarioRepository.Listar().ToList();
        }

        public void Dispose()
        {
            this.Dispose();
        }

        public List<string> Notifications()
        {
            return Notification;
        }
    }
}
