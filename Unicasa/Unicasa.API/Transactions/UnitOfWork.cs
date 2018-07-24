
using Unicasa.API.Persistence;

namespace Unicasa.API.Transactions
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly UnicasaContext _context;

        public UnitOfWork(UnicasaContext context)
        {
            _context = context;
        }

        public void Commit()
        {
            _context.SaveChanges();
        }
    }
}
