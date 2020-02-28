using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using VendingMachine.Data;
using VendingMachine.Helpers;
using VendingMachine.Interface;

namespace VendingMachine.Concrete
{
    public class TestVendingMachine : IVendingMachine
    {
        private readonly ITransactionProcessor _transactionProcessor;

        public TestVendingMachine(ITransactionProcessor transactionProcessor)
        {
            _transactionProcessor = transactionProcessor;
        }

        public void Start()
        {
            _transactionProcessor.ProcessPayment();
            _transactionProcessor.ProcessProducts();
            _transactionProcessor.ValidateTransaction();
            _transactionProcessor.ProcessTransaction();
            FinishTransaction();

            Console.ReadLine();
            Console.ReadLine();
            Console.ReadLine();
        }

        public void FinishTransaction()
        {
            _transactionProcessor.Reset();
            Start();
        }

        public void CancelOrder(int orderId)
        {
            _transactionProcessor.Reset();
        }
    }
}
