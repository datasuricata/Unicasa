using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Unicasa.Domain.Helper
{
    public static class UnicasaExtensions
    {
        public static string ConvertToMD5(this string passWord)
        {
            if (string.IsNullOrEmpty(passWord)) return "";
            var password = (passWord += "|2d331cca-f6c0-40c0-bb43-6e32989c2881");
            var md5 = System.Security.Cryptography.MD5.Create();
            var data = md5.ComputeHash(Encoding.Default.GetBytes(password));
            var sbString = new StringBuilder();
            foreach (var t in data)
                sbString.Append(t.ToString("x2"));

            return sbString.ToString().ToUpper();
        }

        public static DateTime GetDateTicket(this DateTime dataAgendamento, List<DateTime> feriados, int arg)
        {
            var datas = new List<ComponenteData>();
            datas.Add(new ComponenteData(1, dataAgendamento, true));
            bool valida = AjustarData(datas, feriados, arg);

            for (int i = 1; i <= arg; i++)
                datas.Add(new ComponenteData(i + 1, dataAgendamento.AddDays(i), true));

            while (valida)
            {
                if (valida)
                    foreach (var data in datas)
                        data.Data = data.Data.AddDays(1);
                else
                    break;

                valida = AjustarData(datas, feriados, arg);
            }

            return datas.Select(x => x.Data).FirstOrDefault();
        }

        public static List<ComponenteData> ValidarData(this List<ComponenteData> datas, List<DateTime> feriados)
        {
            foreach (var data in datas)
            {
                data.Valida = true;

                feriados.ForEach(x =>{if (x == data.Data)data.Valida = false;});

                if (data.Data.DayOfWeek == DayOfWeek.Saturday || data.Data.DayOfWeek == DayOfWeek.Sunday)
                    data.Valida = false;
            }

            return datas;
        }

        public static bool AjustarData(List<ComponenteData> datas, List<DateTime> feriados, int arg)
        {
            int conter = 0;

            foreach (var data in ValidarData(datas, feriados))
                if (data.Valida)
                    conter++;

            if (conter > arg)
                return false;

            return true;
        }
    }

    public class ComponenteData
    {
        public ComponenteData(int dia, DateTime data, bool valida)
        {
            Dia = dia;
            Data = data;
            Valida = valida;
        }

        public ComponenteData()
        {

        }

        public int Dia { get; set; }
        public DateTime Data { get; set; }
        public bool Valida { get; set; }
    }
}
