using System;

namespace VendingMachine
{
    public class BadDenominationException: Exception
    {
        public BadDenominationException()
        {

        }

        public BadDenominationException(string message) : base(message)
        {

        }
    }

    public class UnderPaymentException : Exception
    {
        public UnderPaymentException()
        {

        }

        public UnderPaymentException(string message) : base(message)
        {

        }
    }

    public class InventoryOutOfBoundsException : Exception
    {
        public InventoryOutOfBoundsException()
        {

        }

        public InventoryOutOfBoundsException(string message) : base(message)
        {

        }
    }

    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException()
        {

        }

        public ProductNotFoundException(string message) : base(message)
        {

        }
    }
}