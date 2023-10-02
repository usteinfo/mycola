using System.ComponentModel;
using System.Globalization;

namespace MyCloa.Common.Util;

public static class TypeExtent
{
    /// <summary>
    /// 类型转换
    /// </summary>
    /// <param name="value">要转换的值</param>
    /// <param name="destinationType">转换的目标类型</param>
    /// <returns></returns>
    public static object? To( this object value, Type destinationType)
    {
        return To(value, destinationType, CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// 类型转换
    /// </summary>
    /// <param name="value">要转换的值</param>
    /// <param name="destinationType">转换的目标类型</param>
    /// <param name="culture">区域信息</param>
    /// <returns></returns>
    public static object? To(this object value, Type destinationType, CultureInfo culture)
    {
        var sourceType = value.GetType();

        var destinationConverter = TypeDescriptor.GetConverter(destinationType);
        if (destinationConverter.CanConvertFrom(value.GetType()))
            return destinationConverter.ConvertFrom(null, culture, value);

        var sourceConverter = TypeDescriptor.GetConverter(sourceType);
        if (sourceConverter.CanConvertTo(destinationType))
            return sourceConverter.ConvertTo(null, culture, value, destinationType);

        if (destinationType.IsEnum && value is int)
            return Enum.ToObject(destinationType, (int)value);

        if (!destinationType.IsInstanceOfType(value))
            return Convert.ChangeType(value, destinationType, culture);
        return value;
    }
}