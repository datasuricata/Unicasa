using System;
using System.Collections.Generic;
using Unicasa.Domain.Entities;
using Unicasa.Domain.Helper;

namespace Unicasa.Domain.Arguments
{
    public class FiltroResponse
    {
        public List<Ticket> Tickets { get; set; }
        public Ticket Ticket { get; set; }
    }
}
