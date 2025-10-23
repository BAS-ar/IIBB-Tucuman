using BAS.Padrones.Tucuman;
using Microsoft.Identity.Client;

namespace TestProject1
{
    public class Tests
    {
        Configuracion configuracion;
        Parametros options = new Parametros();
        AcreditanRegistry acreditanRegistry;
        CoeficienteRegistry coeficienteRegistry;
        CalculadoraDeAlicuota calculadoraDeAlicuota;
        IClientesRepository clientesRepository;

        [SetUp]
        public void Setup()
        {
            configuracion = new Configuracion();
            configuracion.AlicuotaEspecial = 0.17;
            configuracion.RazonCoeficiente = 0.5;
            configuracion.CoeficientesParaExistentes = false;
            configuracion.CoeficientesParaInexistentes = false;

            options = new Parametros();
            options.ProvinceCode = "914";

            acreditanRegistry = new AcreditanRegistry();
            acreditanRegistry.Cuit = "20364986352";
            acreditanRegistry.Excento = false;
            acreditanRegistry.Porcentaje = 3.33;
            acreditanRegistry.Convenio = Convenio.Local;
            acreditanRegistry.Denominacion = "izzitech s.a.";
            acreditanRegistry.FechaDesde = new DateTime(2025, 10, 01);
            acreditanRegistry.FechaHasta = new DateTime(2025, 10, 31);

            coeficienteRegistry = new CoeficienteRegistry();
            coeficienteRegistry.Fecha = new DateTime(2025, 10, 31);
            coeficienteRegistry.Coeficiente = 1.0;
            coeficienteRegistry.Porcentaje = 3.5;
            coeficienteRegistry.Cuit = "20364986352";
            coeficienteRegistry.Denominacion = "izzitech s.a.";
            coeficienteRegistry.Excento = false;

            clientesRepository = new ClientesRepositoryMock();
            
            calculadoraDeAlicuota = new CalculadoraDeAlicuota(clientesRepository, configuracion, options);
        }

        [Test]
        public void CorrectStringGeneration()
        {
            var padron = new PadronRegistry(acreditanRegistry, 3.5);

            var result = padron.ToString();
            Assert.That(result, Is.EqualTo("P;01102025;01102025;31102025;20364986352;D;;N;3.50;;"));
        }

        [Test]
        public void ExcentoShouldReturnZeroAliquot()
        {
            acreditanRegistry.Excento = true;
            calculadoraDeAlicuota.CargarAcreditanRegistry(acreditanRegistry);
            calculadoraDeAlicuota.CargarCoeficientesRegistry(coeficienteRegistry);
            var result = calculadoraDeAlicuota.CalcularAlicuota();
            Assert.That(result.Alicuota, Is.EqualTo(0));
        }

        [Test]
        public void ExistentesFalseInexistentesFalseLocalAlicuotaCompleta()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesFalseInexistentesFalseMultilateralAlicuotaPor05()
        {
            Assert.Fail();
        }


        [Test]
        public void ExistentesTrueInexistentesFalseLocalAlicuotaCompleta()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesFalseMultilateralSedeTucumanAlicuotaPor05()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesFalseMultilateralSinSedeConvenioIgualACeroAlicuotaEspecial()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesFalseMultilateralSinSedeConvenioMayorACeroAlicuotaCoeficientePorParametrizacion()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesFalseInexistentesTrueLocalAlicuotaCompleta()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesFalseInexistentesTrueNoEsLocalAlicuotaPor05()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesFalseInexistentesTrueEnCoeficientesMayorACeroAlicuotaCoeficientePorParametrizacion()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesFalseInexistentesTrueEnCoeficientesIgualACeroAlicuotaEspecial()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesTrueLocalAlicuotaCompleta()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesTrueNoEsLocalSedeTucumanAlicuotaPor05()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesTrueNoEsLocalSinSedeTucumanNoEstaEnCoeficientes()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesTrueEnCoeficientesIgualACeroAlicuotaEspecial()
        {
            Assert.Fail();
        }

        [Test]
        public void ExistentesTrueInexistentesTrueEnCoeficientesMayorACeroAlicuotaCoeficientePorParametrizacion()
        {
            Assert.Fail();
        }
    }
}
