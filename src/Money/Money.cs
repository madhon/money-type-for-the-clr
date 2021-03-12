namespace System
{
    using Diagnostics;
    using Globalization;

    /// <summary>
    /// Represents a decimal amount of a specific <see cref="Currency"/>.
    /// </summary>
    [Serializable]
    [DebuggerDisplay("{getDebugView()}")]
    public readonly struct Money : IEquatable<Money>,
                          IComparable<Money>,
                          IFormattable,
                          IConvertible,
                          IComparable
    {
        /// <summary>
        /// A zero value of money, regardless of currency.
        /// </summary>
        public static readonly Money Zero = new Money(0);

        /// <summary>
        /// A source of randomness for stochastic rounding.
        /// </summary>
        [ThreadStatic]
        private static Random _rng;

        /// <summary>
        /// The amount by which <see cref="_decimalFraction"/> has been scaled up to be a whole number.
        /// </summary>
        private const decimal FractionScale = 1E9M;

        /// <summary>
        /// The <see cref="Core.Money.Currency"/> this amount represents money in.
        /// </summary>
        private readonly Currency? _currency;

        /// <summary>
        /// The whole units of currency.
        /// </summary>
        private readonly long _units;

        /// <summary>
        /// The fractional units of currency.
        /// </summary>
        private readonly int _decimalFraction;

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct equal to <paramref name="value"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        public Money(decimal value)
        {
            checkValue(value);

            _units = (long)value;
            _decimalFraction = (int)decimal.Round((value - _units) * FractionScale);

            if (_decimalFraction >= FractionScale)
            {
                _units += 1;
                _decimalFraction -= (int)FractionScale;
            }

            _currency = Currency.FromCurrentCulture();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct equal to <paramref name="value"/> 
        /// in <paramref name="currency"/>.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <param name="currency">
        /// The currency.
        /// </param>
        public Money(decimal value, Currency currency)
            : this(value)
        {
            _currency = currency;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Money"/> struct.
        /// </summary>
        /// <param name="units">
        /// The units.
        /// </param>
        /// <param name="fraction">
        /// The fraction.
        /// </param>
        /// <param name="currency">
        /// The currency.
        /// </param>
        private Money(long units, int fraction, Currency currency)
        {
            _units = units;
            _decimalFraction = fraction;
            _currency = currency;
        }

        /// <summary>
        /// Gets the <see cref="Core.Money.Currency"/> which this money value is specified in.
        /// </summary>
        public Currency Currency => _currency.GetValueOrDefault(Currency.FromCurrentCulture());

        /// <summary>
        /// Implicitly converts a <see cref="Byte"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(byte value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="sbyte"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(sbyte value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="Single"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(float value) => new Money((decimal)value);

        /// <summary>
        /// Implicitly converts a <see cref="double"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(double value) => new Money((decimal)value);

        /// <summary>
        /// Implicitly converts a <see cref="decimal"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(decimal value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="Money"/> value to <see cref="decimal"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="decimal"/> value which this <see cref="Money"/> value is equivalent to.
        /// </returns>
        public static implicit operator decimal(Money value) => value.computeValue();

        /// <summary>
        /// Implicitly converts a <see cref="short"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(short value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="int"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(int value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="long"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(long value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="ushort"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(ushort value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="uint"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(uint value) => new Money(value);

        /// <summary>
        /// Implicitly converts a <see cref="ulong"/> value to <see cref="Money"/> with no <see cref="Currency"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// A <see cref="Money"/> value with no <see cref="Currency"/> specified.
        /// </returns>
        public static implicit operator Money(ulong value) => new Money(value);

        /// <summary>
        /// A negation operator for a <see cref="Money"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The additive inverse (negation) of the given <paramref name="value"/>.
        /// </returns>
        public static Money operator -(Money value) => new Money(-value._units, -value._decimalFraction, value.Currency);

        /// <summary>
        /// An identity operator for a <see cref="Money"/> value.
        /// </summary>
        /// <param name="value">
        /// The value.
        /// </param>
        /// <returns>
        /// The given <paramref name="value"/>.
        /// </returns>
        public static Money operator +(Money value) => value;

        /// <summary>
        /// An addition operator for two <see cref="Money"/> values.
        /// </summary>
        /// <param name="left">
        /// The left operand.
        /// </param>
        /// <param name="right">
        /// The right operand.
        /// </param>
        /// <returns>
        /// The sum of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the currencies of <paramref name="left"/> and <paramref name="right"/> are not equal.
        /// </exception>
        public static Money operator +(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw differentCurrencies();
            }

            var fractionSum = left._decimalFraction + right._decimalFraction;

            var overflow = 0L;
            var fractionSign = Math.Sign(fractionSum);
            var absFractionSum = Math.Abs(fractionSum);

            if (absFractionSum >= FractionScale)
            {
                overflow = fractionSign;
                absFractionSum -= (int)FractionScale;
                fractionSum = fractionSign * absFractionSum;
            }

            var newUnits = left._units + right._units + overflow;

            if (fractionSign < 0 && Math.Sign(newUnits) > 0)
            {
                newUnits -= 1;
                fractionSum = (int)FractionScale - absFractionSum;
            }

            return new Money(newUnits,
                             fractionSum,
                             left.Currency);
        }

        /// <summary>
        /// A subtraction operator for two <see cref="Money"/> values.
        /// </summary>
        /// <param name="left">
        /// The left operand.
        /// </param>
        /// <param name="right">
        /// The right operand.
        /// </param>
        /// <returns>
        /// The difference of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the currencies of <paramref name="left"/> and <paramref name="right"/> are not equal.
        /// </exception>
        public static Money operator -(Money left, Money right)
        {
            if (left.Currency != right.Currency)
            {
                throw differentCurrencies();
            }

            return left + -right;
        }

        /// <summary>
        /// A product operator for a <see cref="Money"/> value and a <see cref="decimal"/> value.
        /// </summary>
        /// <param name="left">
        /// The left operand.
        /// </param>
        /// <param name="right">
        /// The right operand.
        /// </param>
        /// <returns>
        /// The product of <paramref name="left"/> and <paramref name="right"/>.
        /// </returns>
        public static Money operator *(Money left, decimal right) => new Money((decimal)left * right, left.Currency);

        public static Money operator /(Money left, decimal right) => new Money((decimal)left / right, left.Currency);

        public static bool operator ==(Money left, Money right) => left.Equals(right);

        public static bool operator !=(Money left, Money right) => !left.Equals(right);

        public static bool operator >(Money left, Money right) => left.CompareTo(right) > 0;

        public static bool operator <(Money left, Money right) => left.CompareTo(right) < 0;

        public static bool operator >=(Money left, Money right) => left.CompareTo(right) >= 0;

        public static bool operator <=(Money left, Money right) => left.CompareTo(right) <= 0;

        public static bool TryParse(string s, out Money money)
        {
            money = Zero;

            if (s == null)
            {
                return false;
            }

            s = s.Trim();

            if (s.Length == 0)
            {
                return false;
            }

            Currency? currency = null;

            // Check for currency symbol (e.g. $, £)
            if (!Currency.TryParse(s.Substring(0, 1), out var currencyValue))
            {
                // Check for currency ISO code (e.g. USD, GBP)
                if (s.Length > 2 && Currency.TryParse(s.Substring(0, 3), out currencyValue))
                {
                    s = s.Substring(3);
                    currency = currencyValue;
                }
            }
            else
            {
                s = s.Substring(1);
                currency = currencyValue;
            }

            if (!decimal.TryParse(s, out var value))
            {
                return false;
            }

            money = currency != null ? new Money(value, currency.Value) : new Money(value);

            return true;
        }

        public Money Round(RoundingPlaces places, MidpointRoundingRule rounding = MidpointRoundingRule.ToEven) => Round(places, rounding, out var remainder);

        public Money Round(RoundingPlaces places, MidpointRoundingRule rounding, out Money remainder)
        {
            long unit;

            var placesExponent = getExponentFromPlaces(places);
            var fraction = roundFraction(placesExponent, rounding, out unit);
            var units = _units + unit;
            remainder = new Money(0, _decimalFraction - fraction, Currency);

            return new Money(units, fraction, Currency);
        }

        private int roundFraction(int exponent, MidpointRoundingRule rounding, out long unit)
        {
            var denominator = FractionScale / (decimal)Math.Pow(10, exponent);
            var fraction = _decimalFraction / denominator;

            switch (rounding)
            {
                case MidpointRoundingRule.ToEven:
                    fraction = Math.Round(fraction, MidpointRounding.ToEven);
                    break;
                case MidpointRoundingRule.AwayFromZero:
                    {
                        var sign = Math.Sign(fraction);
                        fraction = Math.Abs(fraction);           // make positive
                        fraction = Math.Floor(fraction + 0.5M);  // round UP
                        fraction *= sign;                        // reapply sign
                        break;
                    }
                case MidpointRoundingRule.TowardZero:
                    {
                        var sign = Math.Sign(fraction);
                        fraction = Math.Abs(fraction);           // make positive
                        fraction = Math.Floor(fraction + 0.5M);  // round DOWN
                        fraction *= sign;                        // reapply sign
                        break;
                    }
                case MidpointRoundingRule.Up:
                    fraction = Math.Floor(fraction + 0.5M);
                    break;
                case MidpointRoundingRule.Down:
                    fraction = Math.Ceiling(fraction - 0.5M);
                    break;
                case MidpointRoundingRule.Stochastic:
                    if (_rng == null)
                    {
                        _rng = new MersenneTwister();
                    }

                    var coinFlip = _rng.NextDouble();

                    if (coinFlip >= 0.5)
                    {
                        goto case MidpointRoundingRule.Up;
                    }

                    goto case MidpointRoundingRule.Down;
                default:
                    throw new ArgumentOutOfRangeException("rounding");
            }

            fraction *= denominator;

            if (fraction >= FractionScale)
            {
                unit = 1;
                fraction = fraction - (int)FractionScale;
            }
            else
            {
                unit = 0;
            }

            return (int)fraction;
        }

        private static int getExponentFromPlaces(RoundingPlaces places)
        {
            switch (places)
            {
                case RoundingPlaces.Zero:
                    return 0;
                case RoundingPlaces.One:
                    return 1;
                case RoundingPlaces.Two:
                    return 2;
                case RoundingPlaces.Three:
                    return 3;
                case RoundingPlaces.Four:
                    return 4;
                case RoundingPlaces.Five:
                    return 5;
                case RoundingPlaces.Six:
                    return 6;
                case RoundingPlaces.Seven:
                    return 7;
                case RoundingPlaces.Eight:
                    return 8;
                case RoundingPlaces.Nine:
                    return 9;
                default:
                    throw new ArgumentOutOfRangeException("places");
            }
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (397 * _units.GetHashCode()) ^ _currency.GetHashCode();
            }
        }
        
        public override bool Equals(object obj)
        {
            if (!(obj is Money))
            {
                return false;
            }

            var other = (Money)obj;
            return Equals(other);
        }

        public bool Equals(Money other)
        {
            checkCurrencies(other);

            return _units == other._units &&
                   _decimalFraction == other._decimalFraction;
        }

        public int CompareTo(Money other)
        {
            checkCurrencies(other);

            var unitCompare = _units.CompareTo(other._units);

            return unitCompare == 0
                       ? _decimalFraction.CompareTo(other._decimalFraction)
                       : unitCompare;
        }

        public override string ToString() => computeValue().ToString("C", (IFormatProvider)_currency ?? NumberFormatInfo.CurrentInfo);

        public string ToString(string format) => computeValue().ToString(format, (IFormatProvider)_currency ?? NumberFormatInfo.CurrentInfo);

        public string ToString(string format, IFormatProvider formatProvider) => computeValue().ToString(format, formatProvider);

        int IComparable.CompareTo(object obj)
        {
            if (obj is Money)
            {
                return CompareTo((Money)obj);
            }

            throw new InvalidOperationException("Object is not a " + GetType() + " instance.");
        }

        public TypeCode GetTypeCode() => TypeCode.Object;

        public bool ToBoolean(IFormatProvider provider)
        {
            if (_units == 0 && _decimalFraction == 0) return true;
            return false;
        }

        public char ToChar(IFormatProvider provider) => throw new NotSupportedException();

        public sbyte ToSByte(IFormatProvider provider) => (sbyte)computeValue();

        public byte ToByte(IFormatProvider provider) => (byte)computeValue();

        public short ToInt16(IFormatProvider provider) => (short)computeValue();

        public ushort ToUInt16(IFormatProvider provider) => (ushort)computeValue();

        public int ToInt32(IFormatProvider provider) => (int)computeValue();

        public uint ToUInt32(IFormatProvider provider) => (uint)computeValue();

        public long ToInt64(IFormatProvider provider) => (long)computeValue();

        public ulong ToUInt64(IFormatProvider provider) => (ulong)computeValue();

        public float ToSingle(IFormatProvider provider) => (float)computeValue();

        public double ToDouble(IFormatProvider provider) => (double)computeValue();

        public decimal ToDecimal(IFormatProvider provider) => computeValue();

        public DateTime ToDateTime(IFormatProvider provider) => throw new NotSupportedException();

        public string ToString(IFormatProvider provider) => ((decimal)this).ToString(provider);

        public object ToType(Type conversionType, IFormatProvider provider) => throw new NotSupportedException();

        private decimal computeValue() => _units + _decimalFraction / FractionScale;

        private static InvalidOperationException differentCurrencies() =>
            new InvalidOperationException("Money values are in different " +
                                          "currencies. Convert to the same " +
                                          "currency before performing " +
                                          "operations on the values.");

        private static void checkValue(decimal value)
        {
            if (value < long.MinValue || value > long.MaxValue)
            {
                throw new ArgumentOutOfRangeException("value",
                                                      value,
                                                      "Money value must be between " +
                                                      long.MinValue + " and " +
                                                      long.MaxValue);
            }
        }

        private void checkCurrencies(Money other)
        {
            if (other.Currency != Currency)
            {
                throw differentCurrencies();
            }
        }

        private string getDebugView() =>
            ToString() +
            $" ({ToDecimal(CultureInfo.CurrentCulture)} {(Currency == Currency.None ? "<Unspecified Currency>" : Currency.Name)})";
    }
}