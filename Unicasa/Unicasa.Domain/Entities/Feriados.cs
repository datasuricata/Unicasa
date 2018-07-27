using System;
using Unicasa.Domain.Entities.Base;

namespace Unicasa.Domain.Entities
{
    public class Feriados : BaseEntity
    {
        public string Titulo { get; set; }
        public DateTime DataFeriado  { get; set; }
        public bool Ativo { get; set; }

        public Feriados()
        {

        }

        public static Feriados Registrar(Feriados feriado)
        {
            if (string.IsNullOrEmpty(feriado.Titulo))
                return null;

            feriado.GerarId();

            return feriado;
        }

        public static Feriados Editar(Feriados feriado, Feriados editado)
        {
            if (!string.IsNullOrEmpty(editado.Titulo))
                feriado.Titulo = editado.Titulo;

            if(feriado.DataFeriado != null)
                feriado.DataFeriado = feriado.DataFeriado;

            feriado.Ativo = editado.Ativo;

            return feriado;
        }
    }
}
