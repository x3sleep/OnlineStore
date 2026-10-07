namespace Lab2.Interfaces.Shapes;

public class Rectangle : IDrawable, IShape
{
    public Rectangle(double width, double height)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height);
        Width = width;
        Height = height;
    }

    public double Width { get; }
    public double Height { get; }

    public void Draw() => Console.WriteLine($"Рисую прямоугольник {Width} x {Height}");

    public double GetArea() => Width * Height;

    public double GetPerimeter() => 2 * (Width + Height);

    public override string ToString() => $"Прямоугольник ({Width} x {Height})";
}
