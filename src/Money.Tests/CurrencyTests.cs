namespace System.Tests
{
    using System;
    using System.Globalization;
    using Shouldly;

    public class CurrencyTests
    {
        public void CurrencyFromCurrentCultureEqualsCurrentCultureCurrency()
        {
            //// NOTE: I think this test could fail in certain cultures...
            var currency1 = new Currency(new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol);
            var currency2 = Currency.FromCurrentCulture();

            currency2.Name.ShouldBe(currency1.Name);
            currency2.Symbol.ShouldBe(currency1.Symbol);
            currency2.Iso3LetterCode.ShouldBe(currency1.Iso3LetterCode);
            currency2.IsoNumericCode.ShouldBe(currency1.IsoNumericCode);
        }

        public void CurrencyFromSpecificCultureInfoIsCorrect()
        {
            var currency = Currency.FromCultureInfo(new CultureInfo(1052));

            currency.IsoNumericCode.ShouldBe(8);
        }

        public void CurrencyFromSpecificIsoCodeIsCorrect()
        {
            var currency = Currency.FromIso3LetterCode("EUR");

            currency.IsoNumericCode.ShouldBe(978);
        }

        public void CurrencyHasValueEquality()
        {
            var currency1 = new Currency("USD");
            var currency2 = new Currency("USD");
            object boxedCurrency2 = currency2;

            currency1.Equals(currency2).ShouldBe(true);
            currency1.Equals(boxedCurrency2).ShouldBe(true);
        }
    }
}