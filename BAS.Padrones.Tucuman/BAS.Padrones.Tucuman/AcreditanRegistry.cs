﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{

    internal class AcreditanRegistry
    {
        public string? Cuit {  get; set; }
        
        public bool Excento { get; set; }

        public Convenio Convenio { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public string? Denominacion { get; set; }

        public double? Porcentaje { get; set; }

        public override string ToString()
        {
            return $"{Cuit};{Excento};{Convenio};{FechaDesde};{Denominacion};{Porcentaje}";
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
    }

}
