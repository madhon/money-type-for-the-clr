namespace System
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class MoneyAllocationException : Exception
    {
        private readonly Money _amountToDistribute;
        private readonly Money _distributionTotal;
        private readonly decimal[] _distribution;

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        decimal[] distribution)
        {
            _amountToDistribute = amountToDistribute;
            _distribution = distribution;
            _distributionTotal = distributionTotal;
        }

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        decimal[] distribution,
                                        string message)
            : base(message)
        {
            _amountToDistribute = amountToDistribute;
            _distribution = distribution;
            _distributionTotal = distributionTotal;
        }

        public MoneyAllocationException(Money amountToDistribute,
                                        Money distributionTotal,
                                        decimal[] distribution,
                                        string message,
                                        Exception inner)
            : base(message, inner)
        {
            _amountToDistribute = amountToDistribute;
            _distribution = distribution;
            _distributionTotal = distributionTotal;
        }

        protected MoneyAllocationException(SerializationInfo info,
                                           StreamingContext context)
            : base(info, context)
        {
            _amountToDistribute = (Money)info.GetValue("_amountToDistribute",
                                                       typeof(Money));
            _distributionTotal = (Money)info.GetValue("_distributionTotal",
                                                      typeof(Money));
            _distribution = (decimal[])info.GetValue("_distribution",
                                                     typeof(decimal[]));
        }

        public decimal[] Distribution => _distribution;

        public Money DistributionTotal => _distributionTotal;

        public Money AmountToDistribute => _amountToDistribute;
    }
}
