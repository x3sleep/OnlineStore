namespace Lab2.Interfaces.Devices.Segregated;

public class Printer : IPrinter
{
    public void Print(string document) => Console.WriteLine($"Принтер печатает: {document}");
}
