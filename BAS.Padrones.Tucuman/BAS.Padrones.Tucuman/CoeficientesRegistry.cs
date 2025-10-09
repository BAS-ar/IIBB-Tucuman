using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal class CoeficientesRegistry
    {
        public string? Cuit { get; set; }

        public bool Excento { get; set; }

        public Double? Coeficiente { get; set; }

        public DateTime Fecha { get; set; }

        public string? Denominacion { get; set; }

        public double? Porcentaje { get; set; }

        public override string ToString()
        {
            return $"{Cuit};{Excento};{Coeficiente};{Fecha};{Denominacion};{Porcentaje}";
        }

        public void ParsePorcentaje(string line)
        {
            var porcentaje = line.Substring(line.Length - 5, 5).Trim();

            if (porcentaje == "-----")
            {
                Porcentaje = null;
            }
            else
            {
                Porcentaje = Double.Parse(porcentaje, CultureInfo.InvariantCulture);
            }
        }

        public void ParseCoeficiente(string coeficiente)
        {
            if (coeficiente == "-.----")
            {
                Coeficiente = null;
            }
            else
            {
                Coeficiente = Double.Parse(coeficiente, CultureInfo.InvariantCulture);
            }
        }
    }
}
