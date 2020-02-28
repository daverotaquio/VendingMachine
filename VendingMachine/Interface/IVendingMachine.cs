namespace VendingMachine.Interface
{
    public interface IVendingMachine
    {
        void CancelOrder(int orderId);
        void Start();
    }
}