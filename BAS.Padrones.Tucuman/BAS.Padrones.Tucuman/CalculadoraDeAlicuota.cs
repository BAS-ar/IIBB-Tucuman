using Microsoft.Extensions.Configuration;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http.Headers;
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

            if (_acreditanRegistry is null)
            {
                retorno.Regimen = Regimen.Retencion;
                retorno.Alicuota = SoloEnCoeficientes().Alicuota;
                return retorno;
            }

            if (_acreditanRegistry!.Excento)
            {
                retorno.Regimen = Regimen.Percepcion;
                retorno.Alicuota = 0;
                return retorno;
            }

            if (_acreditanRegistry.Convenio == Convenio.Local)
            {
                retorno.Regimen = Regimen.Percepcion;
                retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value;
                return retorno;
            }

            if (_acreditanRegistry.Convenio == Convenio.Multilateral)
            {
                if (!_coeficientesParaExistentes)
                {
                    retorno.Regimen = Regimen.Retencion;
                    retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value * 0.5;
                    return retorno;
                }

                bool localClient = _clientesRepository.EsLocal(_acreditanRegistry.Cuit!, _options.ProvinceCode!);
                if (localClient || _coeficienteRegistry == null)
                {
                    retorno.Regimen = Regimen.Retencion;
                    retorno.Alicuota = _acreditanRegistry.Porcentaje!.Value * 0.5;
                    return retorno;
                }

                if (_coeficienteRegistry.Coeficiente > 0)
                {
                    retorno.Regimen = Regimen.Retencion;
                    retorno.Alicuota = _coeficienteRegistry.Porcentaje!.Value * _razonCoeficiente;
                    return retorno;
                }

                retorno.Regimen = Regimen.Retencion;
                retorno.Alicuota = _alicuotaEspecial;
                return retorno;

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
