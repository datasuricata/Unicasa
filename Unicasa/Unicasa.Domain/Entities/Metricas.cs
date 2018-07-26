using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Metricas : BaseEntity
    {
        public string AgendamentosPorDia { get; set; }
        public string DiasMinimosEntrega { get; set; }

        public Metricas()
        {

        }
    }
}
