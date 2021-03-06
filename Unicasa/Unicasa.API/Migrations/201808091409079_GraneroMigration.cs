namespace Unicasa.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class GraneroMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Feriados",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        Titulo = c.String(maxLength: 100, unicode: false),
                        DataFeriado = c.DateTime(nullable: false),
                        Ativo = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Cargas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        UsuarioImportacao = c.String(maxLength: 100, unicode: false),
                        NomeArquivo = c.String(maxLength: 100, unicode: false),
                        DataProcessamento = c.DateTime(nullable: false),
                        Observacao = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
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
                        CargaId = c.String(nullable: false, maxLength: 100, unicode: false),
                        Entregue = c.Boolean(nullable: false),
                        Importado = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Metricas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        AgendamentosPorDia = c.Int(nullable: false),
                        DiasMinimosEntrega = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        ImportacaoId = c.String(maxLength: 100, unicode: false),
                        Cliente = c.String(maxLength: 100, unicode: false),
                        Chave = c.String(maxLength: 100, unicode: false),
                        Titulo = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        Detalhe = c.String(maxLength: 100, unicode: false),
                        Operador = c.String(maxLength: 100, unicode: false),
                        Observacao = c.String(maxLength: 100, unicode: false),
                        DataAgendamento = c.DateTime(),
                        DataEntrega = c.DateTime(),
                        DataColeta = c.DateTime(),
                        TicketState = c.Int(nullable: false),
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
            
            CreateTable(
                "dbo.Agendamentos",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        DataAgendamento = c.DateTime(),
                        Agendados = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            //admin key qawsed123yhuj
            Sql("INSERT INTO dbo.Usuarios (Id, NomeCompleto, Email, Senha, UserRole) VALUES ('E178FC1D-F905-4113-9580-1C139F35AEB7', 'master' ,'admin', 'F540DFD9907A4E4784BA8D205EDC8F88', 99)");
            Sql("INSERT INTO dbo.Metricas (Id, AgendamentosPorDia, DiasMinimosEntrega) VALUES ('1', '4' ,'2')");
        }
        
        public override void Down()
        {
            DropTable("dbo.Agendamentos");
            DropTable("dbo.Usuarios");
            DropTable("dbo.Tickets");
            DropTable("dbo.Metricas");
            DropTable("dbo.Importacoes");
            DropTable("dbo.Cargas");
            DropTable("dbo.Feriados");
        }
    }
}
