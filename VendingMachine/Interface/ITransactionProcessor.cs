using VendingMachine.Data;

namespace VendingMachine.Interface
{
    public interface ITransactionProcessor
    {
        void AddProduct(Product product);
        void CancelTransaction();
        void ProcessTransaction();
        void ProcessPayment();
        void ProcessProducts();
        void Reset();
        void ValidateTransaction();
    }
}