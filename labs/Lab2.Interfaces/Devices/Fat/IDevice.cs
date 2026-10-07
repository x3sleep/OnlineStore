namespace Lab2.Interfaces.Devices.Fat;

public interface IDevice
{
    void Print(string document);
    void Scan(string document);
    void Fax(string document, string phoneNumber);
}
