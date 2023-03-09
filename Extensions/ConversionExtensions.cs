using System;

namespace Jarloo.Sojurn.Extensions;

public static class ConversionExtensions
{
    public static T To<T>(this object text)
    {
        if (text == null) return default;
        if (text.Equals(DBNull.Value)) return default;
        if (text is string s)
            if (string.IsNullOrWhiteSpace(s))
                return default;

        var type = typeof(T);

        var underlyingType = Nullable.GetUnderlyingType(type) ?? type;
        return (T)Convert.ChangeType(text, underlyingType);
    }

    public static bool IsNull(this object o)
    {
        return o == null;
    }
}