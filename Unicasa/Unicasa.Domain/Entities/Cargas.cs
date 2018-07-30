using System;
using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Cargas : BaseEntity
    {
        public string UsuarioImportacao { get; set; }
        public string NomeArquivo { get; set; }
        public DateTime DataProcessamento { get; set; }
        public string Observacao { get; set; }

        public Cargas()
        {
            DataProcessamento = DateTime.Now;
        }
    }
}
