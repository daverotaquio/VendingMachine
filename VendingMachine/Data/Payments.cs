using System;
using System.Collections.Generic;
using System.Text;

namespace VendingMachine.Data
{
    public interface IPaymentsProvider
    {
        List<decimal> GetPayments();
    }

    public class PaymentsProvider : IPaymentsProvider
    {
        private readonly List<decimal> _payments;

        public PaymentsProvider()
        {
            _payments = new List<decimal>
            {
                100,
                20,
                5
            };
        }

        public List<decimal> GetPayments()
        {
            return _payments;
        }
    }
}
