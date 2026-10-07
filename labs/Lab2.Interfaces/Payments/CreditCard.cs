namespace Lab2.Interfaces.Payments;

public class CreditCard(string number, decimal limit) : IPayable
{
    public decimal Limit { get; private set; } = limit;

    public void Pay(decimal amount)
    {
        if (amount > Limit)
        {
            throw new InvalidOperationException($"Недостаточно средств на карте {Mask()}");
        }

        Limit -= amount;
        Console.WriteLine($"Оплата картой {Mask()}: {amount:C}, доступный лимит {Limit:C}");
    }

    private string Mask() => $"**** {number[^4..]}";
}
