
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Unicasa.API.Controllers.Base;
using Unicasa.API.Persistence;
using Unicasa.API.Persistence.Repositories;
using Unicasa.API.Transactions;
using Unicasa.Domain.Arguments;
using Unicasa.Domain.Arguments.Base;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.API.Controllers
{
    [RoutePrefix("api/gerenciador")]
    public class GerenciadorController : ControllerBase
    {
        private readonly RepositoryTickets repositoryTickets;
        private readonly RepositoryMetricas repositoryMetricas;
        private readonly RepositoriyFeriado repositoriyFeriado;
        private readonly RepositoryImportacao repositoryImportacao;

        private readonly UnicasaContext context;

        public GerenciadorController()
        {
            context = new UnicasaContext();

            repositoryTickets = new RepositoryTickets(context);
            repositoryMetricas = new RepositoryMetricas(context);
            repositoriyFeriado = new RepositoriyFeriado(context);
            repositoryImportacao = new RepositoryImportacao(context);

            uow = new UnitOfWork(context);
            Notification = new List<string>();
        }

        [Route("obterPorId")]
        [HttpGet] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> ObterPorId(string id)
        {
            try
            {
                if (id == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var ticket = repositoryTickets.ObterPorId(id);

                var response = new GerenciadorResponse()
                {
                    Ticket = ticket
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("listar")]
        [HttpGet] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var tickets = repositoryTickets.Listar().Select(x => x).Take(10).ToList();

                var response = new GerenciadorResponse()
                {
                    Tickets = tickets
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("filtrar")]
        [HttpPost] //Retorna FiltroResponse
        public async Task<HttpResponseMessage> Filtrar(FiltroRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = Filtro(request);

                if (response == null)
                {
                    Notification.Add("Ocorreu um erro ao aplicar o filtro, contate o suporte");
                    return null;
                }

                var filtro = new FiltroResponse()
                {
                    Tickets = response
                };

                return await ResponseAsync(filtro);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("adicionar/ticket")]
        [HttpPost] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> Adicionar(GerenciadorRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                request.Ticket = ValidaEntrada(request.Ticket);
                var ticket = repositoryTickets.Adicionar(request.Ticket);

                if (ticket == null)
                {
                    Notification.Add("O Ticket não foi criado, tente novamente");
                    return null;
                }

                var response = new GerenciadorResponse()
                {
                    Ticket = ticket
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("editar")]
        [HttpPut] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> Editar(GerenciadorRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                request.Ticket = ValidaEntrada(request.Ticket);

                var ticket = repositoryTickets.Editar(request.Ticket);

                if (ticket == null)
                {
                    Notification.Add("O Ticket não alterado e salvo no banco, tente novamente");
                    return null;
                }

                var response = new GerenciadorResponse()
                {
                    Ticket = ticket
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("sincronizar/tickets")]
        [HttpGet] //Retorna Bool
        public async Task<HttpResponseMessage> Sincronizar(string id)
        {
            try
            {
                if (id == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                bool sincronizado = false;

                var response = repositoryImportacao.ListarPor(x => x.CargaId == id).ToList();

                List<Ticket> tickets = new List<Ticket>();

                if (response == null)
                {
                    Notification.Add("Tickets não sincronizados, tente novamente");
                    return null;
                }

                response.ForEach(x =>
                {
                    x.Importado = true;

                    var importacao = repositoryImportacao.Editar(x);

                    if (importacao != null)
                    {
                        tickets.Add(new Ticket()
                        {
                            ImportacaoId = importacao.Id,
                            DataAgendamento = null,
                            DataEntrega = null,
                            TicketState = TicketState.Aguardando,
                            Chave = importacao.Pedido,
                            Descricao = importacao.Descricao,
                            Detalhe = importacao.OrdCompra,
                            Titulo = importacao.Lote,
                        });
                    }
                });

                IEnumerable<Ticket> entidades = tickets;

                var sincronizar = repositoryTickets.AdicionarLista(entidades);

                if (sincronizar != null)
                    sincronizado = true;

                return await ResponseAsync(sincronizado);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/GetByChave")]
        [HttpGet] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> AppGetChave(string id)
        {
            try
            {
                if (id == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var tickets = repositoryTickets.ListarPor(x => x.Chave == id).ToList();


                var response = new GerenciadorResponse()
                {
                    Tickets = tickets
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/entrega")]
        [HttpPut] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> AppEntrega(GerenciadorRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                if (!request.TicketIds.Any())
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                //todo refatorar
                var entidades = repositoryTickets.Listar().ToList();

                entidades.ForEach(x =>
                {
                    request.TicketIds.ForEach(s =>
                    {
                        if (x.Chave == s)
                        {
                            x.DataEntrega = DateTime.Now;
                            x.TicketState = TicketState.Entregue;
                        }
                    });
                });

                foreach (var entidade in entidades)
                {
                    var ticket = repositoryTickets.Editar(entidade);

                    if (ticket == null)
                        Notification.Add("Os Ticket não foram atualizados no banco, tente novamente: " + entidade.Id);
                }

                var response = new BaseResponse()
                {
                    Exceptions = Notification,
                    Message = "Carga atualizada."
                };

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/filtrar")]
        [HttpPost] //Retorna FiltroResponse
        public async Task<HttpResponseMessage> AppFiltrar(FiltroRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = Filtro(request);

                if (response == null)
                {
                    Notification.Add("Ocorreu um erro ao aplicar o filtro, contate o suporte");
                    return null;
                }

                var filtro = new FiltroResponse()
                {
                    Tickets = response
                };

                return await ResponseAsync(filtro);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/filtrar/importacoes")]
        [HttpPost] //Retorna FilterGerenciadorResponse
        public async Task<HttpResponseMessage> AppFiltrarImportacoes(FiltroRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var filtrado = new GerenciadorResponse();
                var lista = new List<FilterGerenciadorResponse>();

                var response = Filtro(request);

                if (response == null)
                {
                    Notification.Add("Ocorreu um erro ao aplicar o filtro, contate o suporte");
                    return null;
                }

                var importacoes = repositoryImportacao.Listar().ToList();

                if (importacoes == null)
                {
                    Notification.Add("Ocorreu um erro ao aplicar o filtro, contate o suporte");
                    return null;
                }

                foreach (var importacao in importacoes)
                {
                    foreach (var ticket in response)
                    {
                        if (ticket.ImportacaoId == importacao.Id)
                        {
                            var filtro = new FilterGerenciadorResponse();
                            filtro.Ticket = ticket;
                            filtro.Importacao = importacao;
                            lista.Add(filtro);
                        }
                    }
                }

                filtrado.Filtrados = lista;

                return await ResponseAsync(filtrado);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        #region [ Privates ]

        private Ticket ValidaEntrada(Ticket request)
        {
            var metricas = repositoryMetricas.Listar().FirstOrDefault();
            var entradas = repositoryTickets.ListarPor(x => x.DataAgendamento == DateTime.Now).ToList();
            var feriados = repositoriyFeriado.Listar().Where(x => x.Ativo == true).ToList();

            if (entradas == null)
            {
                Notification.Add("Sem tickets registros na base");
                return null;
            }

            if (metricas == null)
            {
                Notification.Add("Sem metricas registradas na base");
                return null;
            }

            if (feriados == null)
            {
                Notification.Add("Sem feriados registrados na base");
                return null;
            }

            if (entradas.Count() <= metricas.AgendamentosPorDia)
            {
                Notification.Add("Limite de agendamentos por dia: " + entradas.Count() + " de " + metricas.AgendamentosPorDia);
                return null;
            }

            var listaDatas = feriados.Select(x => x.DataFeriado).ToList();

            request.DataAgendamento = UnicasaExtensions.GetDateTicket(request.DataAgendamento.Value, listaDatas, metricas.DiasMinimosEntrega);

            return request;
        }

        private List<Ticket> Filtro(FiltroRequest request)
        {
            var query = repositoryTickets.Listar();

            if (!string.IsNullOrEmpty(request.Chave))
                query.Where(x => x.Chave == request.Chave);

            if (request.DataAgendamento != null)
                query.Where(x => x.DataAgendamento == request.DataAgendamento);

            if (request.DataEntrega != null)
                query.Where(x => x.DataEntrega == request.DataEntrega);

            if (Enum.IsDefined(typeof(TicketState), request.TicketState))
                query.Where(x => x.TicketState == request.TicketState);

            if (request.DataPeriodo == DatePeriod.Semanal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-7)));

            if (request.DataPeriodo == DatePeriod.Quinzenal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-15)));

            if (request.DataPeriodo == DatePeriod.Mensal)
                query.Where(x => x.DataAgendamento == DateTime.Now && x.DataAgendamento == (DateTime.Now.AddDays(-30)));

            return query.ToList();
        }

        #endregion
    }
}

