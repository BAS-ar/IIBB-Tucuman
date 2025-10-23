using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    public interface IClientesRepository
    {
        public bool EsLocal(string cuit, string provinciaCode);
    }
}
