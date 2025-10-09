// 2025-10-09
// Iván Sierra
// BAS Software

using BAS.Padrones.Tucuman;
using System.Diagnostics;

var acreditanFilepath = "C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\ACREDITAN.txt";
var coeficientesFilepath = "C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\archivocoefrg116.txt";

var readerAcreditan = new TucumanAcreditanReader(acreditanFilepath);
var readerCoeficientes = new TucumanCoeficientesReader(coeficientesFilepath);
var outputFile = new StreamWriter("Output.txt");

Stopwatch sw = Stopwatch.StartNew();

Console.CursorVisible = false;
Console.WriteLine($"Leyendo archivo acreditan: {acreditanFilepath}");
List<AcreditanRegistry> padron = readerAcreditan.GetRegistries();
Console.WriteLine($"Leyendo archivo coeficientes: {coeficientesFilepath}");
List<CoeficientesRegistry> coeficientes = readerCoeficientes.GetRegistries();

int i = 0;
foreach (var registry in padron)
{
    Console.SetCursorPosition(0, Console.CursorTop);
    i++;
    // Slows down a lot when looking for coeficients.
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