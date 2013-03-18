using System;
using Xunit;

namespace System.Tests
{
    public class MoneyDistributorTests
    {
        [Fact]
        public void UniformDistributionMustBeBetween0And1()
        {
            MoneyDistributor distributor = new MoneyDistributor(1.0M,
                                                                FractionReceivers.LastToFirst,
                                                                RoundingPlaces.Two);

            Assert.Throws<ArgumentOutOfRangeException>(() => distributor.Distribute(0));
            Assert.Throws<ArgumentOutOfRangeException>(() => distributor.Distribute(1.1M));
        }


        [Fact]
        public void DistributeUniformRatioToLastIsCorrect()
        {
            Money amountToDistribute = 0.05M;

            // two decimal places
            MoneyDistributor distributor = new MoneyDistributor(amountToDistribute,
                                                                FractionReceivers.LastToFirst,
                                                                RoundingPlaces.Two);

            Money[] distribution = distributor.Distribute(0.3M);

            Assert.Equal(3, distribution.Length);
            Assert.Equal(new Money(0.01M), distribution[0]);
            Assert.Equal(new Money(0.02M), distribution[1]);
            Assert.Equal(new Money(0.02M), distribution[2]);

            // seven decimal places
            distributor = new MoneyDistributor(amountToDistribute,
                                               FractionReceivers.LastToFirst,
                                               RoundingPlaces.Seven);

            distribution = distributor.Distribute(0.3M);

            Assert.Equal(3, distribution.Length);
            Assert.Equal(new Money(0.0166666M), distribution[0]);
            Assert.Equal(new Money(0.0166667M), distribution[1]);
            Assert.Equal(new Money(0.0166667M), distribution[2]);
        }

        [Fact]
        public void DistributeNonuniformRatiosToLastIsCorrect()
        {
            Money amountToDistribute = 0.05M;

            MoneyDistributor distributor = new MoneyDistributor(amountToDistribute,
                                                                FractionReceivers.LastToFirst,
                                                                RoundingPlaces.Two);

            Money[] distribution = distributor.Distribute(0.7M, 0.3M);

            Assert.Equal(2, distribution.Length);
            Assert.Equal(0.03, distribution[0]);
            Assert.Equal(0.02, distribution[1]);
        }
    }
}
