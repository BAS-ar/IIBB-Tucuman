using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal class AdapterTucumanBsAs
    {
        private PadronRegistry _registry;

        public AdapterTucumanBsAs(PadronRegistry registry)
        {
            _registry = registry;
        }

        public string GetString()
        {
            return $"{_registry.Cuit};{_registry.Excento};{_registry.Convenio};{_registry.FechaDesde};{_registry.Denominacion};{_registry.Porcentaje}";
        }
    }
}
