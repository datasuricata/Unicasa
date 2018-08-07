namespace Unicasa.Domain.Helper
{
    public enum UserRole
    {
        Varejo = 1,
        Gerente = 2,
        Administrador = 99
    }

    public enum TicketState
    {
        Selecione = 0,
        Aguardando = 1,
        Agendado = 2,
        Entregue = 3
    }

    public enum RequestMethod
    {
        Get = 1,
        Post = 2,
        Put = 3,
        Delete = 4
    }

    public enum DatePeriod
    {
        Selecione = 0,
        Semanal = 1,
        Quinzenal = 15,
        Mensal = 30
    }
}
