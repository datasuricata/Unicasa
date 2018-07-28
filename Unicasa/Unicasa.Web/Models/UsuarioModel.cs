using System.Collections.Generic;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.Web.Models
{
    public class UsuarioModel
    {
        public UsuarioModel()
        {
            Usuarios = new List<Usuario>();
            Usuario = new Usuario();
        }

        public Usuario Usuario { get; set; }
        public List<Usuario> Usuarios { get; set; }
        public List<GenericDropdown> Perfis { get; set; }
    }
}