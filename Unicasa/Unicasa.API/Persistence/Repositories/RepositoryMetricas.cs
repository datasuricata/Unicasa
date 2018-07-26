using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryMetricas : Repository<Metricas, Guid>, IMetricasRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryMetricas(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}