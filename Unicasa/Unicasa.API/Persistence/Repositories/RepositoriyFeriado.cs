using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoriyFeriado : Repository<Feriados>, IFeriadosRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoriyFeriado(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}