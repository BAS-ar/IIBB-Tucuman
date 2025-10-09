using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal static class TypeParser
    {
        public static Double? ParsePorcentaje(string line)
        {
            var porcentaje = line.Substring(line.Length - 5, 5).Trim();

            if (porcentaje == "-----")
            {
                return null;
            }

            return Double.Parse(porcentaje, CultureInfo.InvariantCulture);
        }
    }
}
