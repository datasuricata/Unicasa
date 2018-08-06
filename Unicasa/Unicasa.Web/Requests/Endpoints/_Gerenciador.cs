using Unicasa.Web.Requests.Endpoints.Base;

namespace Unicasa.Dashboard.Requests.Endpoints
{
    public static class _Gerenciador
    {
        public const string Listar = Endpoint.Gerenciador + "listar";
        public const string Filtrar = Endpoint.Gerenciador + "filtrar";
        public const string ObterPorId = Endpoint.Gerenciador + "obterPorId";
        public const string Adicionar = Endpoint.Gerenciador + "adicionar/ticket";
        public const string Editar = Endpoint.Gerenciador + "editar/lote";
        public const string Sincronizar = Endpoint.Gerenciador + "sincronizar/tickets";
        public const string OberPorChave = Endpoint.Gerenciador + "app/GetByChave";
    }
}