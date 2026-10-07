namespace Lab2.Interfaces.Devices.Fat;

public class Scanner : IDevice
{
    public void Print(string document)
    {
    }

    public void Scan(string document) => Console.WriteLine($"Сканер сканирует: {document}");

    public void Fax(string document, string phoneNumber)
    {
    }
}
