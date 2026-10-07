namespace Lab2.Interfaces.Payments;

public static class PaymentProcessor
{
    public static void ProcessPayment(IPayable method, decimal amount)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);
        method.Pay(amount);
    }
}
