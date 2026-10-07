namespace Lab2.Interfaces.Devices.Segregated;

public class MultifunctionDevice : IPrinter, IScanner, IFax
{
    public void Print(string document) => Console.WriteLine($"МФУ печатает: {document}");

    public void Scan(string document) => Console.WriteLine($"МФУ сканирует: {document}");

    public void Fax(string document, string phoneNumber) => Console.WriteLine($"МФУ отправляет факс «{document}» на номер {phoneNumber}");
}
