using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Unicasa.Domain.Interfaces.Repository;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryImportacao : IRepositoryImportacao
    {
        protected readonly UnicasaContext _context;

        public RepositoryImportacao(UnicasaContext context)
        {
            _context = context;
        }

        public void Register(Importacoes entity)
        {
            _context.Importacoes.Add(entity);
            _context.SaveChanges();
        }
    }
}