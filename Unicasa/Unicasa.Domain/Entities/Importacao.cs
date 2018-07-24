namespace Unicasa.Domain.Entities
{
    public class Importacao : BaseEntity
    {
        protected Importacao(string id) : base(id)
        {

        }

        public string Lote { get; set; }
        public string CodTransportadora { get; set; }
        public string Pedido { get; set; }
        public string Descricao { get; set; }
        public string NumVolume { get; set; }
        public string TotalVolume { get; set; }
        public string OrdCompra { get; set; }
        public string Carga { get; set; }
        public string RefItem { get; set; }
        public string Barra { get; set; }
        public string Situcao { get; set; }
        public string Cliente { get; set; }
        public string Endereco { get; set; }
        public string Cidade { get; set; }
        public string UF { get; set; }
        public string Quantidade { get; set; }
        public string Documento { get; set; }
        public string Peso { get; set; }
        public string Cubagem { get; set; }
        public string SubFamilia { get; set; }
        public string Fechamento { get; set; }
        public string Esteira { get; set; }
        public string Expedicao { get; set; }
        public string CpfCnpj { get; set; }
    }
}
