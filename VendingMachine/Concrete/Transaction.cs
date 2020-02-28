using System;
using System.Collections.Generic;
using System.Text;
using VendingMachine.Data;
using VendingMachine.Enum;

namespace VendingMachine.Concrete
{
    public class Transaction
    {
        public List<Product> Products { get; set; } = new List<Product>();
        public List<Inventory> Inventories { get; set; } = new List<Inventory>();
        public decimal Payment { get; set; }
        public TransactionStatus Status { get; set; }
    }
}
