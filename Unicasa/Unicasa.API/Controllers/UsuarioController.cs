using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Transactions;
using Unicasa.Domain.Interfaces.Services;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/usuario")]
    public class UsuarioController : ControllerBase
    {
        private readonly IUsuarioService usuarioService;

        public UsuarioController(IUnitOfWork unitOfWork, IUsuarioService usuarioService) : base(unitOfWork)
        {
            this.usuarioService = usuarioService;
        }

        [Route("adicionar")]
        [HttpPost]
        public async Task<HttpResponseMessage> Adicionar(string request)
        {
            try
            {
                var response = usuarioService.AutenticarUsuario(new Domain.Arguments.AutenticarRequest());

                return await ResponseAsync(response, usuarioService);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }
    }
}