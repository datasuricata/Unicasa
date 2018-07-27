using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryTickets : Repository<Ticket>, ITicketRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryTickets(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}