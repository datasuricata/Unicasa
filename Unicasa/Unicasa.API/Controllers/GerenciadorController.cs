
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
        [HttpGet]
        public async Task<HttpResponseMessage> ObterPorId(string id)
        {
            try
            {
                if (id == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repositoryTickets.ObterPorId(id);
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("listar")]
        [HttpGet]
        public async Task<HttpResponseMessage> Listar()
        {
            try
            {
                var response = repositoryTickets.Listar().ToList();
                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("filtrar")]
        [HttpPost]
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

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("adicionar/ticket")]
        [HttpPost]
        public async Task<HttpResponseMessage> Adicionar(Ticket request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                request = ValidaEntrada(request);
                var response = repositoryTickets.Adicionar(request);

                if (response == null)
                {
                    Notification.Add("O Ticket não foi criado, tente novamente");
                    return null;
                }

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("editar")]
        [HttpPut]
        public async Task<HttpResponseMessage> Editar(Ticket request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repositoryTickets.Editar(request);

                if (response == null)
                {
                    Notification.Add("O Ticket não foi salvo no banco, tente novamente");
                    return null;
                }

                Notification.Add("Ticket Alterado");

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("sincronizar/tickets")]
        [HttpGet]
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

        [Route("app/entrega")]
        [HttpPost]
        public async Task<HttpResponseMessage> AppEntrega(List<string> request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                //todo refatorar
                var response = repositoryTickets.Listar().ToList();

                response.ForEach(x =>
                {
                    request.ForEach(s =>
                    {
                        if (x.Chave == s)
                        {
                            x.DataEntrega = DateTime.Now;
                            x.TicketState = TicketState.Entregue;
                        }
                    });
                });

                IEnumerable<Ticket> tickets = response;

                var update = repositoryTickets.AdicionarLista(tickets);

                if (update == null)
                {
                    Notification.Add("Os Ticket não foram atualizados no banco, tente novamente");
                    return null;
                }

                return await ResponseAsync(response);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/filtrar")]
        [HttpPost]
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

                var filtrado = new FiltroResponse()
                {
                    Tickets = response
                };

                return await ResponseAsync(filtrado);
            }
            catch (Exception ex)
            {
                return await ResponseExceptionAsync(ex);
            }
        }

        [Route("app/GetByChave")]
        [HttpGet]
        public async Task<HttpResponseMessage> AppGetChave(string id)
        {
            try
            {
                if (id == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var response = repositoryTickets.ListarPor(x => x.Chave == id).ToList();


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

