using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    // Padron Bs.As.
    // +----+-----------------------------+------+----------+-------+-------+---------------------------------------------------------------+
    // | Nº | Campo                       | Tipo | Longitud | Desde | Hasta | Descripción                                                   |
    // +----+-----------------------------+------+----------+-------+-------+---------------------------------------------------------------+
    // |  1 | Régimen                     | A    | 1        | 1     | 1     | R o P según se trate del régimen de Retención o Percepción    |
    // |  2 | ";"                         | A    | 1        | 2     | 2     | Separador                                                     |
    // |  3 | Fecha de Publicación        | N    | 8        | 3     | 10    | Formato DDMMAAAA                                              |
    // |  4 | ";"                         | A    | 1        | 11    | 11    | Separador                                                     |
    // |  5 | Fecha de vigencia Desde     | N    | 8        | 12    | 19    | Formato DDMMAAAA                                              |
    // |  6 | ";"                         | A    | 1        | 20    | 20    | Separador                                                     |
    // |  7 | Fecha de vigencia Hasta     | N    | 8        | 21    | 28    | Formato DDMMAAAA                                              |
    // |  8 | ";"                         | A    | 1        | 29    | 29    | Separador                                                     |
    // |  9 | Numero CUIT                 | N    | 11       | 30    | 40    | CUIT del sujeto                                               |
    // | 10 | ";"                         | A    | 1        | 41    | 41    | Separador                                                     |
    // | 11 | Tipo Contribuyente Insc     | A    | 1        | 42    | 42    | "C" Convenio Multilateral - "D" Directo Pcia de Bs.As.        |
    // | 12 | ";"                         | A    | 1        | 43    | 43    | Separador                                                     |
    // | 13 | Marca Alta - Baja - Sujeto  | A    | 1        | 44    | 44    | S o N - "S" indica alta, "B" indica baja                      |
    // | 14 | ";"                         | A    | 1        | 45    | 45    | Separador                                                     |
    // | 15 | Marca cambio alícuota       | A    | 1        | 46    | 46    | S o N - indica si hubo cambio de alícuota                     |
    // | 16 | ";"                         | A    | 1        | 47    | 47    | Separador                                                     |
    // | 17 | Alícuota                    | A    | 4        | 48    | 51    | Formato 9,99                                                  |
    // | 18 | ";"                         | A    | 1        | 52    | 52    | Separador                                                     |
    // | 19 | Nro Grupo                   | N    | 2        | 53    | 54    |                                                               |
    // | 20 | ";"                         | A    | 1        | 55    | 55    | Separador                                                     |
    // +----+-----------------------------+------+----------+-------+-------+---------------------------------------------------------------+

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
