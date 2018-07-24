using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryUsuario : Repository<Usuario, Guid>, IUsuarioRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryUsuario(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}