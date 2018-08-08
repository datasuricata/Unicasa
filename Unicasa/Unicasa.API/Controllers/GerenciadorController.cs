
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
        #region [ PARAMETROS ]

        private readonly RepositoryTickets repositoryTickets;
        private readonly RepositoryMetricas repositoryMetricas;
        private readonly RepositoriyFeriado repositoriyFeriado;
        private readonly RepositoryUsuario repositoryUsuario;
        private readonly RepositoryAgendamentos repositoryAgendamentos;
        private readonly RepositoryImportacao repositoryImportacao;

        private readonly UnicasaContext context;

        #endregion

        public GerenciadorController()
        {
            context = new UnicasaContext();

            repositoryTickets = new RepositoryTickets(context);
            repositoryMetricas = new RepositoryMetricas(context);
            repositoriyFeriado = new RepositoriyFeriado(context);
            repositoryAgendamentos = new RepositoryAgendamentos(context);
            repositoryImportacao = new RepositoryImportacao(context);
            repositoryUsuario = new RepositoryUsuario(context);

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
                var tickets = repositoryTickets.Listar().ToList();

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

        [Route("editar/lote")]
        [HttpPut] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> EditarLote(GerenciadorRequest request)
        {
            try
            {
                if (request == null)
                {
                    Notification.Add("Verifique as informações e tente novamente");
                    return null;
                }

                var entrada = ValidaEntrada(request.Agendamento);
                var response = new BaseResponse();

                if (entrada != null)
                {
                    var query = repositoryTickets.ListarPor(x => x.Chave == request.Chave).ToList();

                    if (query == null)
                    {
                        Notification.Add("Produto não localizado");
                        return null;
                    }

                    foreach (var x in query)
                    {
                        x.DataAgendamento = entrada;
                        x.TicketState = TicketState.Agendado;

                        var ticket = repositoryTickets.Editar(x);

                        if (ticket == null)
                            Notification.Add("O Ticket: " + x.Id + " não alterado e salvo no banco, tente novamente");
                    }

                    var agendamento = repositoryAgendamentos.ListarPor(x => x.DataAgendamento == entrada).FirstOrDefault();

                    if (agendamento != null)
                    {
                        agendamento.Agendados++;
                        repositoryAgendamentos.Editar(agendamento);
                    }
                    else
                    {
                        var agend = new Agendamentos() { DataAgendamento = entrada, Agendados = 1 };
                        repositoryAgendamentos.Adicionar(agend);
                    }

                    response.Message = "Pedido atualizado.";
                }

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
                if (id == null) { Notification.Add("Verifique as informações e tente novamente"); return null; }

                bool sincronizado = false;
                bool existe = false;

                var importacoes = repositoryImportacao.ListarPor(x => x.CargaId == id).ToList();
                var usuarios = repositoryUsuario.Listar().ToList();

                if (usuarios != null)
                    existe = (usuarios.Count() < 20);

                List<Ticket> tickets = new List<Ticket>();

                if (importacoes == null) { Notification.Add("Tickets não sincronizados, tente novamente"); return null; }

                importacoes.ForEach(x =>
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
                            Cliente = importacao.Cliente,
                            Operador = importacao.CpfCnpj.Trim()
                        });
                    }
                });

                var userImports = tickets.Select(x => x.Operador).Distinct();

                foreach (var item in userImports)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        var entity = new Usuario(item, item);
                        ImportarUsuario(entity, existe, usuarios);
                    }
                }

                IEnumerable<Ticket> entidades = tickets;

                var sincronizar = repositoryTickets.AdicionarLista(entidades);

                if (sincronizar != null)
                    sincronizado = true;

                var response = new BaseResponse();

                if (sincronizado)
                    response.Message = "Ordens de serviço sincronizada.";
                else
                    response.Message = string.Empty;

                return await ResponseAsync(response);
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
                var editados = new List<Ticket>();

                entidades.ForEach(x =>
                {
                    request.Tickets.ForEach(s =>
                    {
                        if (x.Id == s.Id)
                        {
                            x.DataEntrega = s.DataEntrega;
                            x.TicketState = TicketState.Entregue;

                            editados.Add(x);
                        }
                    });
                });

                foreach (var entidade in editados)
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

        [Route("app/coleta")]
        [HttpPut] //Retorna GerenciadorResponse
        public async Task<HttpResponseMessage> AppColeta(GerenciadorRequest request)
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
                var editados = new List<Ticket>();

                entidades.ForEach(x =>
                {
                    request.Tickets.ForEach(s =>
                    {
                        if (x.Id == s.Id)
                        {
                            x.DataColeta = s.DataColeta;
                            editados.Add(x);
                        }
                    });
                });

                foreach (var entidade in editados)
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

        private DateTime? ValidaEntrada(DateTime? request)
        {
            var metricas = repositoryMetricas.Listar().FirstOrDefault();

            var feriados = repositoriyFeriado.Listar().Where(x => x.Ativo == true).ToList();

            if (metricas == null) { Notification.Add("Sem metricas registradas na base"); return null; }

            if (feriados == null) { Notification.Add("Sem feriados registrados na base"); return null; }

            var listaDatas = feriados.Select(x => x.DataFeriado).ToList();

            var data = UnicasaExtensions.GetDateTicket(request.Value, listaDatas, metricas.DiasMinimosEntrega);

            var entradas = repositoryAgendamentos.ListarPor(x => x.DataAgendamento == data).FirstOrDefault();

            if (entradas != null)
                if (entradas.Agendados >= metricas.AgendamentosPorDia) { Notification.Add("Limite de agendamentos por dia: " + entradas + " de " + metricas.AgendamentosPorDia); return null; }
            
            return data;
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

        private void ImportarUsuario(Usuario usuario, bool primeiroImport, List<Usuario> usuarios)
        {
            var response = new Usuario();
            usuario.UserRole = UserRole.Varejo;
            usuario.NomeCompleto = usuario.Email;

            if (primeiroImport)
            {
                var existe = repositoryUsuario.Existe(x => x.Email == usuario.Email && x.Senha == usuario.Senha);

                if (!existe)
                {
                    response = repositoryUsuario.Adicionar(usuario);
                }
            }

            else if (!usuarios.Any(x => x.Email == usuario.Email))
                response = repositoryUsuario.Adicionar(usuario);

            //if (response != null)
            //    Notification.Add("Usuário " + usuario.Email + " não importado.");
        }

        #endregion
    }
}

