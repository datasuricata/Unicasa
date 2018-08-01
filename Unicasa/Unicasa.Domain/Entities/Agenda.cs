using System;
using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Agenda : BaseEntity
    {
        public string Titulo { get; set; }
        public DateTime DataFeriado  { get; set; }
        public bool Ativo { get; set; }

        public Agenda()
        {

        }

        public static Agenda Registrar(Agenda agenda)
        {
            if (string.IsNullOrEmpty(agenda.Titulo))
                return null;

            agenda.GerarId();

            return agenda;
        }

        public static Agenda Editar(Agenda agenda, Agenda editado)
        {
            if (!string.IsNullOrEmpty(editado.Titulo))
                agenda.Titulo = editado.Titulo;

            if(agenda.DataFeriado != null)
                agenda.DataFeriado = agenda.DataFeriado;

            agenda.Ativo = editado.Ativo;

            return agenda;
        }
    }
}
