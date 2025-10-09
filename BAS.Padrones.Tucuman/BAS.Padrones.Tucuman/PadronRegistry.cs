using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    public enum Convenio
    {
        None = 0,
        Local = 1,
        Multilateral = 2,
    }

    internal class PadronRegistry
    {
        public string? Cuit {  get; set; }
        
        public bool Excento { get; set; }

        public Convenio Convenio { get; set; }

        public DateTime FechaDesde { get; set; }

        public DateTime FechaHasta { get; set; }

        public string? Denominacion { get; set; }

        public double? Porcentaje { get; set; }

    }

}
