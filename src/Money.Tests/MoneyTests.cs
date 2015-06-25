using System;
using System.Globalization;
using System.Threading;
using Shouldly;

namespace System.Tests
{
    public class MoneyTests
    {
        public void MoneyHasValueEquality()
        {
            var money1 = new Money(101.5M);
            var money2 = new Money(101.5M);

            money2.ShouldBe(money1);
        }

        public void MoneyImplicitlyConvertsFromPrimitiveNumbers()
        {
            Money money;

            Byte byteValue = 50;
            money = byteValue;
            money.ShouldBe(new Money(50));

            SByte sByteValue = 75;
            money = sByteValue;
            money.ShouldBe(new Money(75));

            Int16 int16Value = 100;
            money = int16Value;
            money.ShouldBe(new Money(100));

            Int32 int32Value = 200;
            money = int32Value;
            money.ShouldBe(new Money(200));

            Int64 int64Value = 300;
            money = int64Value;
            money.ShouldBe(new Money(300));

            UInt16 uInt16Value = 400;
            money = uInt16Value;
            money.ShouldBe(new Money(400));

            UInt32 uInt32Value = 500;
            money = uInt32Value;
            money.ShouldBe(new Money(500));

            UInt64 uInt64Value = 600;
            money = uInt64Value;
            money.ShouldBe(new Money(600));

            Single singleValue = 700;
            money = singleValue;
            money.ShouldBe(new Money(700));

            Double doubleValue = 800;
            money = doubleValue;
            money.ShouldBe(new Money(800));

            Decimal decimalValue = 900;
            money = decimalValue;
            money.ShouldBe(new Money(900));
        }

        public void MoneyWholeAmountAdditionIsCorrect()
        {
            // whole number
            Money money1 = 101M;
            Money money2 = 99M;

            (money1 + money2).ShouldBe(new Money(200));
        }

        public void MoneyFractionalAmountAdditionIsCorrect()
        {
            // fractions
            Money money1 = 100.00M;
            Money money2 = 0.01M;

            (money1 + money2).ShouldBe(new Money(100.01M));
        }

        public void MoneyFractionalAmountWithOverflowAdditionIsCorrect()
        {
            // overflow
            Money money1 = 100.999M;
            Money money2 = 0.9M;

            (money1 + money2).ShouldBe(new Money(101.899M));
        }

        public void MoneyNegativeAmountAdditionIsCorrect()
        {
            // negative
            Money money1 = 100.999M;
            Money money2 = -0.9M;

            (money1 + money2).ShouldBe(new Money(100.099M));
        }

        public void MoneyNegativeAmountWithOverflowAdditionIsCorrect()
        {
            // negative overflow
            Money money1 = -100.999M;
            Money money2 = -0.9M;

            (money1 + money2).ShouldBe(new Money(-101.899M));
        }

        public void MoneyWholeAmountSubtractionIsCorrect()
        {
            // whole number
            Money money1 = 101M;
            Money money2 = 99M;

            (money1 - money2).ShouldBe(new Money(2));
        }

        public void MoneyFractionalAmountSubtractionIsCorrect()
        {
            // fractions
            Money money1 = 100.00M;
            Money money2 = 0.01M;

            (money1 - money2).ShouldBe(new Money(99.99M));
        }

        public void MoneyFractionalAmountWithOverflowSubtractionIsCorrect()
        {
            // overflow
            Money money1 = 100.5M;
            Money money2 = 0.9M;

            (money1 - money2).ShouldBe(new Money(99.6M));
        }

        public void MoneyNegativeAmountSubtractionIsCorrect()
        {
            // negative
            Money money1 = 100.999M;
            Money money2 = -0.9M;

            (money1 - money2).ShouldBe(new Money(101.899M));
        }

        public void MoneyNegativeAmountWithOverflowSubtractionIsCorrect()
        {
            // negative overflow
            Money money1 = -100.999M;
            Money money2 = -0.9M;

            (money1 - money2).ShouldBe(new Money(-100.099M));
        }

        public void MoneyScalarWholeMultiplicationIsCorrect()
        {
            Money money = 100.125;

            (money * 5).ShouldBe(new Money(500.625M));
        }

        public void MoneyScalarFractionalMultiplicationIsCorrect()
        {
            Money money = 100.125;

            (money * 0.5M).ShouldBe(new Money(50.0625M));
        }

        public void MoneyScalarNegativeWholeMultiplicationIsCorrect()
        {
            Money money = -100.125;

            (money * 5).ShouldBe(new Money(-500.625M));
        }

        public void MoneyScalarNegativeFractionalMultiplicationIsCorrect()
        {
            Money money = -100.125;
            (money * 0.5M).ShouldBe(new Money(-50.0625M));
        }

        public void MoneyScalarWholeDivisionIsCorrect()
        {
            Money money = 100.125;

            (money / 2).ShouldBe(new Money(50.0625M));
        }

        public void MoneyScalarFractionalDivisionIsCorrect()
        {
            Money money = 100.125;

            (money / 0.5M).ShouldBe(new Money(200.25M));
        }

        public void MoneyScalarNegativeWholeDivisionIsCorrect()
        {
            Money money = -100.125;

            (money / 2).ShouldBe(new Money(-50.0625M));
        }

        public void MoneyScalarNegativeFractionalDivisionIsCorrect()
        {
            Money money = -100.125;
            (money / 0.5M).ShouldBe(new Money(-200.25M));
        }

        public void MoneyEqualOperatorIsCorrect()
        {
            Money money1 = 100.125;
            Money money2 = 100.125;

            money1.ShouldBe(money2);

            money2 = 101.125;
            money1.ShouldNotBe(money2);

            money2 = 100.25;
            money1.ShouldNotBe(money2);
        }

        public void MoneyNotEqualOperatorIsCorrect()
        {
            Money money1 = 100.125;
            Money money2 = 100.125;

            money1.ShouldBe(money2);

            money2 = 101.125;
            money1.ShouldNotBe(money2);

            money2 = 100.25;
            money1.ShouldNotBe(money2);
        }

        public void MoneyLessGreaterThanEqualOperatorIsCorrect()
        {
            Money money1 = 100.125;
            Money money2 = 100.125;

            money1.ShouldBeLessThanOrEqualTo(money2);

            money2 = 101.125;

            money2.ShouldBeGreaterThanOrEqualTo(money1);
            money1.ShouldBeLessThanOrEqualTo(money2);

            money2 = 100.25;
            money2.ShouldBeGreaterThanOrEqualTo(money1);
            money1.ShouldBeLessThanOrEqualTo(money2);
        }

        public void MoneyPrintsCorrectly()
        {
            var previousCulture = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            var money = new Money(100.125M, Currency.Usd);
            var formattedMoney = money.ToString();
            formattedMoney.ShouldBe("$100.13");
            Thread.CurrentThread.CurrentCulture = previousCulture;
        }

        public void MoneyOperationsInvolvingDifferentCurrencyAllFail()
        {
            var money1 = new Money(101.5M, Currency.Aud);
            var money2 = new Money(98.5M, Currency.Cad);
            Money m;
            Boolean b;

            Should.Throw<InvalidOperationException>(() => { m = money1 + money2; });
            Should.Throw<InvalidOperationException>(() => { m = money1 - money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 == money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 != money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 > money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 < money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 >= money2; });
            Should.Throw<InvalidOperationException>(() => { b = money1 <= money2; });
        }

        public void MoneyTryParseIsCorrect()
        {
            var usd = "USD123.45";
            var gbp = "£123.45";
            var cad = "CAD123.45";

            Money actual;

            var result = Money.TryParse(usd, out actual);
            result.ShouldBe(true);
            actual.ShouldBe(new Money(123.45M, Currency.Usd));

            result = Money.TryParse(gbp, out actual);
            result.ShouldBe(true);
            actual.ShouldBe(new Money(123.45M, Currency.Gbp));

            result = Money.TryParse(cad, out actual);
            result.ShouldBe(true);
            actual.ShouldBe(new Money(123.45M, Currency.Cad));
        }
    }
}
