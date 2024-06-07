using System;

namespace Macrobert.ResultPattern
{
    public class Error : IEquatable<Error>
    {
        public static readonly Error None = new Error(string.Empty, string.Empty);
        public static readonly Error NullValue = new Error("Error.NullValue", "The specified result value is null.");

        public Error(string code, string message)
        {
            Code = code;
            Message = message;
        }

        public string Code { get; }

        public string Message { get; }

        public static implicit operator string(Error error) => error.Code;

        public static bool operator ==(Error a, Error b)
        {
            if (a is null && b is null)
            {
                return true;
            }

            if (a is null || b is null)
            {
                return false;
            }

            return a.Equals(b);
        }

        public static bool operator !=(Error a, Error b) => !(a == b);

        public virtual bool Equals(Error other)
        {
            if (other is null)
            {
                return false;
            }

            return Code == other.Code && Message == other.Message;
        }

        public override bool Equals(object obj) => obj is Error error && Equals(error);

        public override int GetHashCode() => HashCodeHelper.Combine(Code, Message);

        public override string ToString() => Code;
    }

    public static class HashCodeHelper // HashCode.Combine not available in .net 2.0
    {
        public static int Combine(params object[] values)
        {
            const int seed = 17;
            const int multiplier = 31;
            int hash = seed;

            foreach (var value in values)
            {
                hash = (hash * multiplier) + (value?.GetHashCode() ?? 0);
            }

            return hash;
        }
    }

}

