namespace System.Tests
{
    using System;
    using Shouldly;

    public class MoneyDistributorTests
    {
        public void UniformDistributionMustBeBetween0And1()
        {
            var distributor = new MoneyDistributor(1.0M, FractionReceivers.LastToFirst, RoundingPlaces.Two);

            Should.Throw<ArgumentOutOfRangeException>( () => distributor.Distribute(0));
            Should.Throw<ArgumentOutOfRangeException>(() => distributor.Distribute(1.1M));
        }

        public void DistributeUniformRatioToLastIsCorrect()
        {
            Money amountToDistribute = 0.05M;

            // two decimal places
            var distributor = new MoneyDistributor(amountToDistribute, FractionReceivers.LastToFirst, RoundingPlaces.Two);

            var distribution = distributor.Distribute(0.3M);

            distribution.Length.ShouldBe(3);
            distribution[0].ShouldBe(new Money(0.01M));
            distribution[1].ShouldBe(new Money(0.02M));
            distribution[2].ShouldBe(new Money(0.02M));

            // seven decimal places
            distributor = new MoneyDistributor(amountToDistribute,
                                               FractionReceivers.LastToFirst,
                                               RoundingPlaces.Seven);

            distribution = distributor.Distribute(0.3M);

            distribution.Length.ShouldBe(3);
            distribution[0].ShouldBe(new Money(0.0166666M));
            distribution[1].ShouldBe(new Money(0.0166667M));
            distribution[2].ShouldBe(new Money(0.0166667M));
        }

        private void DistributeNonuniformRatiosToLastIsCorrect()
        {
            Money amountToDistribute = 0.05M;

            var distributor = new MoneyDistributor(amountToDistribute, FractionReceivers.LastToFirst, RoundingPlaces.Two);

            var distribution = distributor.Distribute(0.7M, 0.3M);

            distribution.Length.ShouldBe(2);
            distribution[0].ShouldBe(0.03);
            distribution[1].ShouldBe(0.02);
        }
    }
}