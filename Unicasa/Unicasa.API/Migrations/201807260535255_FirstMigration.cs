namespace Unicasa.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FirstMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Importacoes",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        Lote = c.String(maxLength: 100, unicode: false),
                        CodTransportadora = c.String(maxLength: 100, unicode: false),
                        Pedido = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        NumVolume = c.String(maxLength: 100, unicode: false),
                        TotalVolume = c.String(maxLength: 100, unicode: false),
                        OrdCompra = c.String(maxLength: 100, unicode: false),
                        Carga = c.String(maxLength: 100, unicode: false),
                        RefItem = c.String(maxLength: 100, unicode: false),
                        Barra = c.String(maxLength: 100, unicode: false),
                        Situcao = c.String(maxLength: 100, unicode: false),
                        Cliente = c.String(maxLength: 100, unicode: false),
                        Endereco = c.String(maxLength: 100, unicode: false),
                        Cidade = c.String(maxLength: 100, unicode: false),
                        UF = c.String(maxLength: 100, unicode: false),
                        Quantidade = c.String(maxLength: 100, unicode: false),
                        Documento = c.String(maxLength: 100, unicode: false),
                        Peso = c.String(maxLength: 100, unicode: false),
                        Cubagem = c.String(maxLength: 100, unicode: false),
                        SubFamilia = c.String(maxLength: 100, unicode: false),
                        Fechamento = c.String(maxLength: 100, unicode: false),
                        Esteira = c.String(maxLength: 100, unicode: false),
                        Expedicao = c.String(maxLength: 100, unicode: false),
                        CpfCnpj = c.String(maxLength: 100, unicode: false),
                        Entregue = c.Boolean(nullable: false),
                        Agendado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Usuarios",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        NomeCompleto = c.String(maxLength: 100, unicode: false),
                        Email = c.String(maxLength: 100, unicode: false),
                        Senha = c.String(maxLength: 100, unicode: false),
                        UserRole = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Usuarios");
            DropTable("dbo.Importacoes");
        }
    }
}
