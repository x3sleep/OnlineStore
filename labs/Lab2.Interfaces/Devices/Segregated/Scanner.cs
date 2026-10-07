namespace Lab2.Interfaces.Devices.Segregated;

public class Scanner : IScanner
{
    public void Scan(string document) => Console.WriteLine($"Сканер сканирует: {document}");
}
