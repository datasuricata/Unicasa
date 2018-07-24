﻿using System;
using Unicasa.API.Persistence.Repositories.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories;

namespace Unicasa.API.Persistence.Repositories
{
    public class RepositoryImportacao : Repository<Importacao, Guid>, IImportacaoRepository
    {
        protected readonly UnicasaContext _context;

        public RepositoryImportacao(UnicasaContext context) : base(context)
        {
            _context = context;
        }
    }
}