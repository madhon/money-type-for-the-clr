using System;
using System.Globalization;
using Xunit;

namespace System.Tests
{
    public class CurrencyTests
    {
        [Fact]
        public void CurrencyFromCurrentCultureEqualsCurrentCultureCurrency()
        {
            // NOTE: I think this test could fail in certain cultures...
            var currency1 = new Currency(new RegionInfo(CultureInfo.CurrentCulture.LCID).ISOCurrencySymbol);
            var currency2 = Currency.FromCurrentCulture();

            Assert.Equal(currency1.Name, currency2.Name);
            Assert.Equal(currency1.Symbol, currency2.Symbol);
            Assert.Equal(currency1.Iso3LetterCode, currency2.Iso3LetterCode);
            Assert.Equal(currency1.IsoNumericCode, currency2.IsoNumericCode);
        }

        [Fact]
        public void CurrencyFromSpecificCultureInfoIsCorrect()
        {
            var currency = Currency.FromCultureInfo(new CultureInfo(1052));

            Assert.Equal(8, currency.IsoNumericCode);
        }

        [Fact]
        public void CurrencyFromSpecificIsoCodeIsCorrect()
        {
            var currency = Currency.FromIso3LetterCode("EUR");
            
            Assert.Equal(978, currency.IsoNumericCode);
        }

        [Fact]
        public void CurrencyHasValueEquality()
        {
            var currency1 = new Currency("USD");
            var currency2 = new Currency("USD");
            object boxedCurrency2 = currency2;

            Assert.True(currency1.Equals(currency2));
            Assert.True(currency1.Equals(boxedCurrency2));
        }
    }
}
