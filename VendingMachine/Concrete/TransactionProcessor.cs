using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using VendingMachine.Data;
using VendingMachine.Enum;
using VendingMachine.Interface;

namespace VendingMachine.Concrete
{
    public class TransactionProcessor : ITransactionProcessor
    {
        public Transaction _transaction;
        private readonly IDenominationProvider _denominationProvider;
        private readonly IProductsProvider _productsProvider;
        private readonly IInventoryProvider _inventoryProvider;
        private readonly IPaymentsProvider _paymentsProvider;

        public TransactionProcessor(IPaymentsProvider paymentsProvider, IDenominationProvider denominationProvider, IProductsProvider productsProvider, IInventoryProvider inventoryProvider)
        {
            _paymentsProvider = paymentsProvider;
            _denominationProvider = denominationProvider;
            _productsProvider = productsProvider;
            _inventoryProvider = inventoryProvider;
            _transaction = new Transaction();
        }

        public void AddProduct(Product product)
        {
            _transaction.Products.Add(product);
        }

        public void CancelTransaction()
        {
            Reset();
        }

        private void AddPayment(decimal payment)
        {
            if (_denominationProvider.GetDenominations().Any(x => x.Value.Equals(payment)))
            {
                _transaction.Payment += payment;
            }
            else
            {
                throw new BadDenominationException("Denomination not found.");
            }
        }

        public void ValidateTransaction()
        {
            decimal total = _transaction.Products.Sum(x => x.Price);

            if (_transaction.Payment < total)
            {
                throw new UnderPaymentException("Payment is less than the total amount due.");
            }
        }
        public void ProcessTransaction()
        {
            foreach (Inventory inventory in _transaction.Products.SelectMany(product => _inventoryProvider.GetInventories().Where(x => x.ProductId == product.Id)))
            {
                inventory.Quantity--;
            }

            _transaction.Status = TransactionStatus.Complete;
        }

        public void ProcessPayment()
        {
            List<decimal> payments = _paymentsProvider.GetPayments();

            foreach (decimal payment in payments)
            {
                AddPayment(payment);
            }
        }

        public void ProcessProducts()
        {
            List<int> productIds = _productsProvider.GetPurchasedProducts();

            foreach (int productId in productIds)
            {
                if (_productsProvider.GetProducts().Any(x => x.Id.Equals(productId)))
                {
                    Product product = _productsProvider.GetProducts().First(x => x.Id == productId);

                    int currentInventoryQuantity = _inventoryProvider.GetInventories().FirstOrDefault(x => x.ProductId == product.Id)?.Quantity ?? 0;
                    int addedProducts = _transaction.Products.Count(x => x.Id == product.Id);

                    if (addedProducts < currentInventoryQuantity)
                    {
                        AddProduct(product);
                    }
                    else
                    {
                        throw new InventoryOutOfBoundsException("Added products is greater than the quantity in the inventory.");
                    }
                }
                else
                {
                    throw new ProductNotFoundException();
                }
            }
        }

        public void Reset()
        {
            _transaction.Inventories.Clear();
            _transaction.Status = TransactionStatus.New;
            _transaction.Products.Clear();
            _transaction.Payment = 0.0m;
        }
    }
}