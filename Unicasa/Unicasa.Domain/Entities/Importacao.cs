using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Importacao : BaseEntity
    {
        public Importacao()
        {

        }

        public Importacao(string lote, string codTransportadora, string pedido, string descricao, string numVolume, string totalVolume, string ordCompra, string carga, string refItem, string barra, string situcao, string cliente, string endereco, string cidade, string uF, string quantidade, string documento, string peso, string cubagem, string subFamilia, string fechamento, string esteira, string expedicao, string cpfCnpj)
        {
            Id = GerarId();
            Lote = lote;
            CodTransportadora = codTransportadora;
            Pedido = pedido;
            Descricao = descricao;
            NumVolume = numVolume;
            TotalVolume = totalVolume;
            OrdCompra = ordCompra;
            Carga = carga;
            RefItem = refItem;
            Barra = barra;
            Situcao = situcao;
            Cliente = cliente;
            Endereco = endereco;
            Cidade = cidade;
            UF = uF;
            Quantidade = quantidade;
            Documento = documento;
            Peso = peso;
            Cubagem = cubagem;
            SubFamilia = subFamilia;
            Fechamento = fechamento;
            Esteira = esteira;
            Expedicao = expedicao;
            CpfCnpj = cpfCnpj;
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
        public bool Entregue { get; set; }
        public bool Agendado { get; set; }
    }
}
