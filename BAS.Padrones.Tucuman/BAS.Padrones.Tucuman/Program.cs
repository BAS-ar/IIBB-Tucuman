// 2025-10-09
// Iván Sierra
// BAS Software

using BAS.Padrones.Tucuman;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using CommandLine;
using System.Text;

string acreditanFilepath = "";
string coeficientesFilepath = "";
string outputFilepath = "";
string provinceCode = "";

Parser.Default.ParseArguments<Options>(args)
    .WithParsed(o =>
    {
        acreditanFilepath = o.AcreditanFilepath ?? "acreditan.txt";
        coeficientesFilepath = o.CoeficientesFilepath ?? "coeficientes.txt";
        outputFilepath = o.OutputFilepath ?? "output.txt";
        provinceCode = o.ProvinceCode ?? "914";
    });

var readerAcreditan = new TucumanAcreditanReader(acreditanFilepath);
var readerCoeficientes = new TucumanCoeficientesReader(coeficientesFilepath);
var outputFile = new StreamWriter(outputFilepath, false, Encoding.UTF8);

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = 
    $"Data Source={configuration["Server"]};" +
    $"Initial Catalog={configuration["Database"]};" +
    $"User Id={configuration["User"]};" +
    $"Password={configuration["Password"]};" +
    $"TrustServerCertificate=True";

// Access to the database. This is extremely slow. Like 700% slower.
var clienteRepository = new ClientesRepository(connectionString);

Stopwatch sw = Stopwatch.StartNew();
Console.CursorVisible = false;
Console.WriteLine($"Servidor de base de datos: {configuration["Server"]}");
Console.WriteLine($"Leyendo archivo acreditan: {acreditanFilepath}");
List<AcreditanRegistry> padron = readerAcreditan.GetRegistries();
Console.WriteLine($"Leyendo archivo coeficientes: {coeficientesFilepath}");
List<CoeficienteRegistry> coeficientes = readerCoeficientes.GetRegistries();

Console.WriteLine("Buscando coeficientes sin registros en el padrón...");
// This uses 30% of the time
var coeficientesSinPadron = coeficientes.Where(c => !padron.Any(p => p.Cuit == c.Cuit)).ToList();
Console.WriteLine($"Se encontraron {coeficientesSinPadron.Count} coeficientes sin registro en el padrón");
Console.WriteLine("Procesando registros de Acreditan...");

var random = new Random();

int i = 0;
foreach (var registry in padron)
{
    Console.SetCursorPosition(0, Console.CursorTop);
    i++;
    // Slows down a lot when looking for coeficients.
    // could be optimized filtering registries that has no record in Acreditan
    var coeficiente = coeficientes.SingleOrDefault(c => c.Cuit == registry.Cuit);

    var bsasRegistry = new PadronRegistry(registry, coeficiente);
    if (registry.Excento)
    {
        // The flow diagram says to do nothing
        // This is NOT right, i guess. It will be applied default aliquot when not found in Padron afterwards.
        // So, i will apply aliquot = 0
        bsasRegistry.Alicuota = 0;
    }
    else
    {
        if (registry.Convenio == Convenio.Multilateral)
        {
            // Check if client is local
            // bool localClient = random.Next(0, 100) > 50;
            bool localClient = clienteRepository.EsLocal(registry.Cuit!, provinceCode);
            if (localClient || coeficiente == null)
            {
                bsasRegistry.Alicuota = bsasRegistry.Alicuota * 0.5;
            }
            else
            {
                bsasRegistry.Regimen = Regimen.Retencion;

                if (coeficiente.Coeficiente > 0)
                {
                    // RG 116/10: 0.5 * COEFICIENTE * ALICUOTA
                    bsasRegistry.Alicuota = registry.Porcentaje * 0.5 * coeficiente.Coeficiente.Value;
                }
                else
                {
                    bsasRegistry.Alicuota = registry.Porcentaje * 0.175;
                }
            }
        }
    }
    outputFile.WriteLine(bsasRegistry.ToString());
    Console.Write($"Se han procesado {i} registros de {padron.Count} ({(((double)i / (double)padron.Count) * 100).ToString("N0")}%)");
}

Console.WriteLine();
Console.WriteLine("Procesando registros de Coeficientes sin registros en Acreditan...");
i = 0;
foreach (var registry in coeficientesSinPadron)
{
    Console.SetCursorPosition(0, Console.CursorTop);
    i++;

    PadronRegistry bsasRegistry;

    // TODO: Look up database to know if the client is local
    // bool localClient = random.Next(0, 100) > 50;
    bool localClient = clienteRepository.EsLocal(registry.Cuit!, provinceCode);
    if (localClient)
    {
        bsasRegistry = new PadronRegistry(registry, 0.5);
    }
    else
    {
        if (registry.Coeficiente > 0)
        {
            // RG 116/10: 0.5 * COEFICIENTE * ALICUOTA
            bsasRegistry = new PadronRegistry(coeficienteRegistry: registry, aliquotPercentage: 0.5 * registry.Coeficiente.Value);
        }
        else
        {
            // ALICUOTA * 0.175
            bsasRegistry = new PadronRegistry(registry, 0.175);
        }
    }

    // PadronRegistry created with only CoeficienteRegistry are always a Retencion
    // bsasRegistry.Regimen = Regimen.Retencion;
    outputFile.WriteLine(bsasRegistry.ToString());

    Console.Write($"Se han procesado {i} registros de {coeficientesSinPadron.Count} ({(((double)i / (double)coeficientesSinPadron.Count) * 100).ToString("N0")}%)");
}

outputFile.Close();
sw.Stop();

Console.WriteLine();
Console.WriteLine("Listo");
Console.WriteLine($"Procesado en {sw.Elapsed}");
Console.CursorVisible = true;