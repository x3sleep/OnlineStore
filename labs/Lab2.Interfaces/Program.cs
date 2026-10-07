using System.Globalization;
using System.Text;
using Lab2.Interfaces.Devices.Segregated;
using Lab2.Interfaces.Logging;
using Lab2.Interfaces.Movement;
using Lab2.Interfaces.Payments;
using Lab2.Interfaces.Shapes;
using FatDevices = Lab2.Interfaces.Devices.Fat;

Console.OutputEncoding = Encoding.UTF8;
CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("ru-RU");

Header("Задание 1.1. Интерфейс IMovable");
var point = new Point(2, 3);
Console.WriteLine($"Начальные координаты точки: {point}");
IMovable movable = point;
movable.Move(5, -1);
Console.WriteLine($"После Move(5, -1): {point}");

Header("Задание 1.2. Интерфейс IDrawable");
var drawables = new List<IDrawable> { new Circle(5), new Rectangle(4, 6), new Circle(1.5) };
ShapeService.DrawAll(drawables);

Header("Задание 2.1. Интерфейс IShape");
ShapeService.PrintShapeInfo(new Circle(5));
ShapeService.PrintShapeInfo(new Rectangle(4, 6));

Header("Задание 2.2. Интерфейс I3DShape : IShape");
ShapeService.PrintShapeInfo(new Cube(3));

Header("Задание 3.1. «Толстый» интерфейс IDevice");
FatDevices.IDevice fatPrinter = new FatDevices.Printer();
FatDevices.IDevice fatScanner = new FatDevices.Scanner();
fatPrinter.Print("Отчет.docx");
fatPrinter.Scan("Паспорт.pdf");
fatPrinter.Fax("Договор.pdf", "+7 495 000-00-00");
fatScanner.Scan("Паспорт.pdf");
fatScanner.Print("Отчет.docx");
Console.WriteLine("Вызовы Scan/Fax у принтера и Print у сканера ничего не сделали: классы вынуждены реализовывать ненужные методы.");

Header("Задание 3.2–3.3. Разделенные интерфейсы IPrinter, IScanner, IFax");
var printer = new Printer();
var scanner = new Scanner();
var mfu = new MultifunctionDevice();
foreach (var device in new object[] { printer, scanner, mfu })
{
    var abilities = new List<string>();
    if (device is IPrinter) abilities.Add("IPrinter");
    if (device is IScanner) abilities.Add("IScanner");
    if (device is IFax) abilities.Add("IFax");
    Console.WriteLine($"{device.GetType().Name}: {string.Join(", ", abilities)}");
}

PrintAll([printer, mfu], "Отчет.docx");
scanner.Scan("Паспорт.pdf");
mfu.Scan("Паспорт.pdf");
mfu.Fax("Договор.pdf", "+7 495 000-00-00");

Header("Задание 4.1. Интерфейс IPayable");
PaymentProcessor.ProcessPayment(new CreditCard("2200123456781234", 50_000m), 12_499.90m);
PaymentProcessor.ProcessPayment(new Cash(5_000m), 3_250.50m);
try
{
    PaymentProcessor.ProcessPayment(new Cash(100m), 250m);
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Ошибка оплаты: {ex.Message}");
}

Header("Задание 4.2. Интерфейс ILogger");
Console.WriteLine("ConsoleLogger:");
Worker.DoWork(new ConsoleLogger());
var fileLogger = new FileLogger(Path.Combine(AppContext.BaseDirectory, "work.log"));
File.Delete(fileLogger.Path);
Worker.DoWork(fileLogger);
Console.WriteLine($"FileLogger записал в файл {Path.GetFileName(fileLogger.Path)}:");
Console.Write(File.ReadAllText(fileLogger.Path));

static void PrintAll(IEnumerable<IPrinter> printers, string document)
{
    foreach (var item in printers)
    {
        item.Print(document);
    }
}

static void Header(string title)
{
    Console.WriteLine();
    Console.WriteLine($"=== {title} ===");
}
