namespace POS_SYSTEM.Interfaces
{
    public interface IPaymentProcessor
    {
        bool ProcessPayment(decimal amount);
    }
}