// 2025-10-09
// Iván Sierra
// BAS Software

using BAS.Padrones.Tucuman;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;

var acreditanFilepath = "C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\ACREDITAN.txt";
var coeficientesFilepath = "C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\archivocoefrg116.txt";

var readerAcreditan = new TucumanAcreditanReader(acreditanFilepath);
var readerCoeficientes = new TucumanCoeficientesReader(coeficientesFilepath);
var outputFile = new StreamWriter("Output.txt");


var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

Stopwatch sw = Stopwatch.StartNew();
Console.CursorVisible = false;
Console.WriteLine($"Servidor de base de datos: {configuration["Server"]}");
Console.WriteLine($"Leyendo archivo acreditan: {acreditanFilepath}");
List<AcreditanRegistry> padron = readerAcreditan.GetRegistries();
Console.WriteLine($"Leyendo archivo coeficientes: {coeficientesFilepath}");
List<CoeficientesRegistry> coeficientes = readerCoeficientes.GetRegistries();

Console.WriteLine("Buscando coeficientes sin registros en el padrón...");
// This uses 30% of the time
var coeficientesSinPadron = coeficientes.Where(c => !padron.Any(p => p.Cuit == c.Cuit)).ToList();
Console.WriteLine($"Se encontraron {coeficientesSinPadron.Count} coeficientes sin registro en el padrón");

int i = 0;
foreach (var registry in padron)
{
    Console.SetCursorPosition(0, Console.CursorTop);
    i++;
    // Slows down a lot when looking for coeficients.
    // could be optimized filtering registries that has no record in Acreditan
    var coeficiente = coeficientes.SingleOrDefault(c => c.Cuit == registry.Cuit);
    var bsasRegitry = new PadronRegistry(registry, coeficiente);
    outputFile.WriteLine(bsasRegitry.ToString());
    Console.Write($"Se han procesado {i} registros de {padron.Count} ({(((double)i / (double)padron.Count) * 100).ToString("N0")}%)");
}

outputFile.Close();
sw.Stop();

Console.WriteLine("");
Console.WriteLine("Listo");
Console.WriteLine($"Procesado en {sw.Elapsed}");
Console.CursorVisible = true;