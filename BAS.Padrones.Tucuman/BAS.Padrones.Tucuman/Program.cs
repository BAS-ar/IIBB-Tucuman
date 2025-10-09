// 2025-10-09
// Iván Sierra
// BAS Software

using BAS.Padrones.Tucuman;

var readerAcreditan = new TucumanAcreditanReader("C:\\Users\\admin\\Documents\\Dev\\IIBB Tucuman\\Padrón Tucuman\\ACREDITAN.txt");

List<AcreditanRegistry> padron = readerAcreditan.GetRegistries();

foreach(var registry in padron)
{
    var bsasRegitry = new PadronRegistry(registry);
    Console.WriteLine(bsasRegitry);
}