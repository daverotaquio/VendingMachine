using System.Collections.Generic;
using VendingMachine.Enum;

namespace VendingMachine.Data
{
    public class Denomination
    {
        public decimal Value { get; set; }
        public DenominationType DenominationType { get; set; }
    }

    public interface IDenominationProvider
    {
        List<Denomination> GetDenominations();
    }

    public class DenominationProvider : IDenominationProvider
    {
        private readonly List<Denomination> _denominations;

        public DenominationProvider()
        {
            _denominations = new List<Denomination>
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
            };
        }

        public List<Denomination> GetDenominations()
        {
            return _denominations;
        }
    }
}