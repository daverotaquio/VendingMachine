using System;
using VendingMachine.Concrete;
using VendingMachine.Data;
using VendingMachine.Interface;

namespace VendingMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            IVendingMachine vendingMachine = new TestVendingMachine(new TransactionProcessor(new PaymentsProvider(),new DenominationProvider(), new ProductsProvider(), new InventoryProvider()));

            vendingMachine.Start();
        }
    }
}
