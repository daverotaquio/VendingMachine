using System.Collections.Generic;
using AutoFixture;
using AutoFixture.Dsl;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using VendingMachine.Concrete;
using VendingMachine.Data;
using VendingMachine.Enum;
using VendingMachine.Interface;

namespace VendingMachine.Test
{
    public class TransactionProcessorTest
    {
        private readonly Mock<IVendingMachine> _vendingMachineMock;
        private readonly Mock<ITransactionProcessor> _transactionProcessorMock;
        private readonly Mock<IDenominationProvider> _denominationProvider;
        private readonly Mock<IPaymentsProvider> _paymentsProviderMock;
        private readonly Mock<IProductsProvider> _productsProviderMock;
        private readonly Mock<IInventoryProvider> _inventoryProviderMock;

        public TransactionProcessorTest()
        {
            _inventoryProviderMock = new Mock<IInventoryProvider>();
            _productsProviderMock = new Mock<IProductsProvider>();
            _denominationProvider = new Mock<IDenominationProvider>();
            _vendingMachineMock = new Mock<IVendingMachine>();
            _paymentsProviderMock = new Mock<IPaymentsProvider>();
            _transactionProcessorMock = new Mock<ITransactionProcessor>();
        }

        #region Setup
        [SetUp]
        public void SetupTest()
        {
            _denominationProvider.Setup(x => x.GetDenominations())
                .Returns(new List<Denomination>
                {
                    new Denomination
                    {
                        DenominationType = DenominationType.Cash,
                        Value = 100
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Cash,
                        Value = 50
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Cash,
                        Value = 20
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Coin,
                        Value = 10
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Coin,
                        Value = 5
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Coin,
                        Value = 1
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Coin,
                        Value = 0.50m
                    },
                    new Denomination
                    {
                        DenominationType = DenominationType.Coin,
                        Value = 0.25m
                    }
                });

            _productsProviderMock.Setup(x => x.GetProducts())
                .Returns(new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Name = "Coke",
                        Price = 25m,
                        InventoryQuantity = 5
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "Pepsi",
                        Price = 35m,
                        InventoryQuantity = 5
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "Soda",
                        Price = 45m,
                        InventoryQuantity = 5
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "Chocolate Bar",
                        Price = 20.25m,
                        InventoryQuantity = 5
                    },
                    new Product
                    {
                        Id = 5,
                        Name = "Chewing Gum",
                        Price = 10.50m,
                        InventoryQuantity = 5
                    },
                    new Product
                    {
                        Id = 6,
                        Name = "Bottled Water",
                        Price = 15m,
                        InventoryQuantity = 5
                    }
                });

            _inventoryProviderMock.Setup(x => x.GetInventories())
                .Returns(new List<Inventory>
                {
                    new Inventory
                    {
                        ProductId = 1,
                        Quantity = 5
                    },
                    new Inventory
                    {
                        ProductId = 2,
                        Quantity = 5
                    },
                    new Inventory
                    {
                        ProductId = 3,
                        Quantity = 5
                    },
                    new Inventory
                    {
                        ProductId = 4,
                        Quantity = 5
                    },
                    new Inventory
                    {
                        ProductId = 5,
                        Quantity = 5
                    },
                    new Inventory
                    {
                        ProductId = 6,
                        Quantity = 5
                    }
                });
        }
        #endregion

        [TestCase]
        public void WhenDenominationIsWrong_ThenItShouldNotBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    7
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, null, null);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().Throw<BadDenominationException>();
        }

        [TestCase]
        public void WhenDenominationIsCorrect_ThenItShouldBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    1
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, null, null);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
        }

        [TestCase]
        public void WhenAddingAProductThatDoesNotExists_ThenItShouldNotBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    1
                });

            _productsProviderMock.Setup(x => x.GetPurchasedProducts())
                .Returns(new List<int>
                {
                    1,
                    2,
                    10
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, _productsProviderMock.Object, _inventoryProviderMock.Object);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
            transactionProcessor.Invoking(x => x.ProcessProducts()).Should().Throw<ProductNotFoundException>();
        }

        [TestCase]
        public void WhenAddingAProductThatExists_ThenItShouldBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    1
                });

            _productsProviderMock.Setup(x => x.GetPurchasedProducts())
                .Returns(new List<int>
                {
                    1,
                    2,
                    3
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, _productsProviderMock.Object, null);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
            transactionProcessor.Invoking(x => x.ProcessProducts()).Should().NotThrow<ProductNotFoundException>();
        }

        [TestCase]
        public void WhenDenominationIsCorrectAndAddingAProductThatExists_ThenItShouldBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    1
                });

            _productsProviderMock.Setup(x => x.GetPurchasedProducts())
                .Returns(new List<int>
                {
                    1,
                    2,
                    3
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, _productsProviderMock.Object, null);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
            transactionProcessor.Invoking(x => x.ProcessProducts()).Should().NotThrow<ProductNotFoundException>();
        }

        [TestCase]
        public void WhenPaymentIsLowerThanTheProductsPurchasedAndAddingAProductThatExists_ThenItShouldNotBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    1
                });

            _productsProviderMock.Setup(x => x.GetPurchasedProducts())
                .Returns(new List<int>
                {
                    1,
                    2,
                    3
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, _productsProviderMock.Object, _inventoryProviderMock.Object);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
            transactionProcessor.Invoking(x => x.ProcessProducts()).Should().NotThrow<ProductNotFoundException>();
            transactionProcessor.Invoking(x => x.ValidateTransaction()).Should().Throw<UnderPaymentException>();
        }

        [TestCase]
        public void WhenPaymentIsCorrectAndAddingAProductThatExists_ThenItShouldBeAcceptedByTheTransactionProcessor()
        {
            _paymentsProviderMock.Setup(x => x.GetPayments())
                .Returns(() => new List<decimal>
                {
                    100,
                    20,
                    1
                });

            _productsProviderMock.Setup(x => x.GetPurchasedProducts())
                .Returns(new List<int>
                {
                    1,
                    2,
                    3
                });

            var transactionProcessor = new TransactionProcessor(_paymentsProviderMock.Object, _denominationProvider.Object, _productsProviderMock.Object, null);

            transactionProcessor.Invoking(x => x.ProcessPayment()).Should().NotThrow<BadDenominationException>();
            transactionProcessor.Invoking(x => x.ProcessProducts()).Should().NotThrow<ProductNotFoundException>();
            transactionProcessor.Invoking(x => x.ValidateTransaction()).Should().NotThrow<UnderPaymentException>();
        }
    }
}