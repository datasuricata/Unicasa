namespace Unicasa.API.Migrations
{
    using System;
    using System.Data.Entity.Migrations;

    public partial class Startup : DbMigration
    {
        public override void Up()
        {
            Sql("INSERT INTO dbo.Usuarios (Id, NomeCompleto, Email, Senha, UserRole) VALUES ('E178FC1D-F905-4113-9580-1C139F35AEB7', 'master' ,'admin', '03C1DBAA940DF84CA0726143046A2340', 99)");
        }

        public override void Down()
        {

        }
    }
}
//123456