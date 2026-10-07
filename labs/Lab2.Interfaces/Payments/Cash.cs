namespace Lab2.Interfaces.Payments;

public class Cash(decimal received) : IPayable
{
    public void Pay(decimal amount)
    {
        if (amount > received)
        {
            throw new InvalidOperationException("Внесенной суммы наличных недостаточно");
        }

        Console.WriteLine($"Оплата наличными: {amount:C}, внесено {received:C}, сдача {received - amount:C}");
    }
}
