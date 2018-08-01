using Unicasa.Web.Requests.Endpoints.Base;

namespace Unicasa.Dashboard.Requests.Endpoints
{
    public static class _Importacao
    {
        public const string Importar = Endpoint.Importacao + "importar";
        public const string ImportarUnitario = Endpoint.Importacao + "importar/registro";
        public const string Listar = Endpoint.Importacao + "listar";
        public const string ListarCargas = Endpoint.Importacao + "cargas";
        public const string Excluir = Endpoint.Importacao + "excluir";
        public const string ObterPorId = Endpoint.Importacao + "getById";
    }
}
