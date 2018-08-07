using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class AgendamentoRepository : Repository<Agendamentos>, IAgendamentoRepository
    {
        protected readonly UnicasaContext _context;

        public AgendamentoRepository(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}