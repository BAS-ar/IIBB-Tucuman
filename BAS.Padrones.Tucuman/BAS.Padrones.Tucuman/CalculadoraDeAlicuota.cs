using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAS.Padrones.Tucuman
{
    internal class CalculadoraDeAlicuota
    {
        IClientesRepository _clientesRepository;
        AcreditanRegistry? _acreditanRegistry;
        CoeficienteRegistry? _coeficienteRegistry;
        Configuracion _configuracion;
        Parametros _options;
        double _razonCoeficiente;
        double _alicuotaEspecial;
        bool _coeficientesParaExistentes = false;
        bool _coeficientesParaInexistentes = false;

        public CalculadoraDeAlicuota(IClientesRepository clientesRepository, Configuracion configuracion, Parametros options)
        {
            _clientesRepository = clientesRepository;
            _configuracion = configuracion;
            _options = options;
        }

        public void CargarAcreditanRegistry(AcreditanRegistry? acreditanRegistry)
        {
            _acreditanRegistry = acreditanRegistry;
        }

        public void CargarCoeficientesRegistry(CoeficienteRegistry? coeficienteRegistry)
        {
            _coeficienteRegistry = coeficienteRegistry;
        }

        public RetornoCalculadora CalcularAlicuota()
        {
            var retorno = new RetornoCalculadora();
            retorno.Regimen = Regimen.Percepcion;

            if (_acreditanRegistry is not null)
            {
                if (_acreditanRegistry!.Excento)
                {
                    retorno.Alicuota = 0;
                    return retorno;
                }

                if (_acreditanRegistry.Convenio == Convenio.Multilateral)
                {
                    bool localClient = _clientesRepository.EsLocal(_acreditanRegistry.Cuit!, _options.ProvinceCode!);
                    if (localClient || _coeficienteRegistry == null)
                    {
                        retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value * _razonCoeficiente;
                    }
                    else
                    {
                        retorno.Regimen = Regimen.Retencion;

                        if (_coeficienteRegistry.Coeficiente > 0)
                        {
                            retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value * 0.5 * _coeficienteRegistry.Coeficiente.Value;
                        }
                        else
                        {
                            retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value * 0.175;
                        }
                    }

                }
            }
            else
            {
                retorno.Alicuota = SoloEnCoeficientes().Alicuota;
            }
            return retorno;
        }

        private RetornoCalculadora SoloEnCoeficientes()
        {
            var retorno = new RetornoCalculadora();
            
            if (_coeficienteRegistry!.Coeficiente == 0)
            {
                retorno.Alicuota = _alicuotaEspecial;
                return retorno;
            }

            retorno.Alicuota = _coeficienteRegistry.Coeficiente!.Value * _razonCoeficiente;
            return retorno;
        }
    }
}
