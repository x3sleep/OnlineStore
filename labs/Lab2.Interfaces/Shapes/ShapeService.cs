namespace Lab2.Interfaces.Shapes;

public static class ShapeService
{
    public static void DrawAll(List<IDrawable> shapes)
    {
        foreach (var shape in shapes)
        {
            shape.Draw();
        }
    }

    public static void PrintShapeInfo(IShape shape)
    {
        Console.WriteLine($"{shape}: площадь = {shape.GetArea():F2}, периметр = {shape.GetPerimeter():F2}");
        if (shape is I3DShape solid)
        {
            Console.WriteLine($"{shape}: объем = {solid.GetVolume():F2}");
        }
    }
}
