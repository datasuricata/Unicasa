﻿using System;
using Unicasa.Domain.Entities.Base;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Entities
{
    public class Ticket : BaseEntity
    {
        public string ImportacaoId { get; set; }
        public string Cliente { get; set; }
        public string Chave { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public string Detalhe { get; set; }
        public string Operador { get; set; }
        public string Observacao { get; set; }

        public DateTime? DataAgendamento { get; set; }
        public DateTime? DataEntrega { get; set; }
        public DateTime? DataColeta { get; set; }

        public TicketState TicketState { get; set; }

        public Ticket()
        {

        }
    }
}