namespace Lab2.Interfaces.Devices.Fat;

public class Printer : IDevice
{
    public void Print(string document) => Console.WriteLine($"Принтер печатает: {document}");

    public void Scan(string document)
    {
    }

    public void Fax(string document, string phoneNumber)
    {
    }
}
