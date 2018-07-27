using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Metricas : BaseEntity
    {
        public int AgendamentosPorDia { get; set; }
        public int DiasMinimosEntrega { get; set; }

        public Metricas()
        {

        }
    }
}
