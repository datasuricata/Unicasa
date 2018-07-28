using Unicasa.Web.Requests.Endpoints.Base;

namespace Unicasa.Dashboard.Requests.Endpoints
{
    public static class _Feriado
    {
        public const string Adicionar = Endpoint.Feriado + "adicionar";
        public const string Importar = Endpoint.Feriado + "importar";
        public const string ObterPorId = Endpoint.Feriado + "obterPorId";
        public const string Listar = Endpoint.Feriado + "listar";
        public const string Editar = Endpoint.Feriado + "editar";
        public const string Excluir = Endpoint.Feriado + "excluir";
    }
}