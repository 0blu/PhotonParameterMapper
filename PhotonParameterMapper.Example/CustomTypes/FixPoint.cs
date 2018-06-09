using System.Globalization;

namespace PhotonParameterMapper.Example.CustomTypes
{
    public struct FixPoint
    {
        public const long MaxIntegerValue = 922337203685477L;
        public const long MinIntegerValue = -922337203685477L;

        public const long InternalFaktor = 10000L;
        public double FloatValue => (double)InternalValue / InternalFaktor;
        public long InternalValue
        {
            get;
        }

        private FixPoint(long internalValue)
        {
            InternalValue = internalValue;
        }

        public static FixPoint FromInternalValue(long internalValue)
        {
            return new FixPoint(internalValue);
        }

        public static FixPoint FromIntegerValue(long iValue)
        {
            if (iValue > MaxIntegerValue)
                iValue = MaxIntegerValue;
            if (iValue < MinIntegerValue)
                iValue = MinIntegerValue;
            return new FixPoint(iValue * InternalFaktor);
        }

        public override string ToString()
        {
            return FloatValue.ToString(CultureInfo.InvariantCulture);
        }
    }
}
