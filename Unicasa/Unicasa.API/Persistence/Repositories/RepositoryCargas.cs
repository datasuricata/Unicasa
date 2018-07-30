using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryCargas : Repository<Cargas>, ICargasRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryCargas(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}