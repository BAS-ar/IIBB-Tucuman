using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal class ClientesRepositoryMock : IClientesRepository
    {
        public bool EsLocal(string cuit, string provinciaCode)
        {
            if (cuit == "20364986352" && provinciaCode == "914")
            {
                return true;
            }
            return false;
        }
    }
}
