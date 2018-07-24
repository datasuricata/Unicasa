using System;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Interfaces.Repositories.Base;

namespace Unicasa.Domain.Interfaces.Repositories
{
    public interface IImportacaoRepository : IRepository<Importacao,Guid>
    {
        Importacao Register(Importacao entity);
    }
}
