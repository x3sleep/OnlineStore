namespace Lab2.Interfaces.Shapes;

public class Cube : I3DShape
{
    public Cube(double edge)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(edge);
        Edge = edge;
    }

    public double Edge { get; }

    public double GetArea() => 6 * Edge * Edge;

    public double GetPerimeter() => 12 * Edge;

    public double GetVolume() => Edge * Edge * Edge;

    public override string ToString() => $"Куб (a = {Edge})";
}
