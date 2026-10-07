namespace Lab2.Interfaces.Shapes;

public class Circle : IDrawable, IShape
{
    public Circle(double radius)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(radius);
        Radius = radius;
    }

    public double Radius { get; }

    public void Draw() => Console.WriteLine($"Рисую круг радиусом {Radius}");

    public double GetArea() => Math.PI * Radius * Radius;

    public double GetPerimeter() => 2 * Math.PI * Radius;

    public override string ToString() => $"Круг (r = {Radius})";
}
