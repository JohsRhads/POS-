using System;
using System.Collections.Generic;
using System.Text;

namespace POS_SYSTEM.Interfaces
{
    public class PaymentProcessor
    {
        public interface IPaymentProcessor
        {
            bool ProcessPayment(decimal amount);
        }
    }
}
