using System;

namespace jjm.one.RabbitMqClientWrapper.util;

/// <summary>
/// This class contains some functions to convert from/to a <see cref="TimeSpan"/> object.
/// </summary>
internal static class TimeSpanTools
{
    #region internal static extension functions

    /// <summary>
    /// Converts an <see cref="int"/> object to a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <returns></returns>
    internal static TimeSpan MillisecondsToTimeSpan(this int milliseconds)
    {
        // init temp
        var temp = milliseconds;
        
        var d = temp / 86400000;
        temp %= 86400000;
        var h = temp / 3600000;
        temp %= 3600000;
        var m = temp / 60000;
        temp %= 60000;
        var s = temp / 1000;
        temp %= 1000;

        return new TimeSpan(days: d, hours: h, minutes: m, seconds: s, milliseconds: temp);
    }

    #endregion
}