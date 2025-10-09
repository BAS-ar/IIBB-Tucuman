// 2025-10-09
// Iván Sierra
// BAS Software

using BAS.Padrones.Tucuman;
using System.Diagnostics;

var readerAcreditan = new TucumanAcreditanReader("C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\ACREDITAN.txt");
var readerCoeficientes = new TucumanCoeficientesReader("C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\archivocoefrg116.txt");

List<AcreditanRegistry> padron = readerAcreditan.GetRegistries();
List<CoeficientesRegistry> coeficientes = readerCoeficientes.GetRegistries();

Stopwatch sw = Stopwatch.StartNew();

int i = 0;

foreach (var registry in padron)
{
    Console.SetCursorPosition(0, Console.CursorTop);
    i++;
    // Slows down a lot when looking for coeficients.
    var coeficiente = coeficientes.SingleOrDefault(c => c.Cuit == registry.Cuit);
    var bsasRegitry = new PadronRegistry(registry, coeficiente);
    Console.Write($"Processed {i} registries");
}

sw.Stop();

Console.WriteLine("");
Console.WriteLine("Done.");
Console.WriteLine($"Processed in {sw.Elapsed}");