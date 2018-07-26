namespace Unicasa.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SecondMigration : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Metricas",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        AgendamentosPorDia = c.String(maxLength: 100, unicode: false),
                        DiasMinimosEntrega = c.String(maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 100, unicode: false),
                        ImportacaoId = c.String(maxLength: 100, unicode: false),
                        Titulo = c.String(maxLength: 100, unicode: false),
                        Descricao = c.String(maxLength: 100, unicode: false),
                        Detalhe = c.String(maxLength: 100, unicode: false),
                        DataAgendamento = c.DateTime(nullable: false),
                        DataEntrega = c.DateTime(nullable: false),
                        TicketState = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Tickets");
            DropTable("dbo.Metricas");
        }
    }
}
