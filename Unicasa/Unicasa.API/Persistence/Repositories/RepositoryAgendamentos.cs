using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryAgendamentos : Repository<Agendamentos>, IAgendamentoRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryAgendamentos(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}