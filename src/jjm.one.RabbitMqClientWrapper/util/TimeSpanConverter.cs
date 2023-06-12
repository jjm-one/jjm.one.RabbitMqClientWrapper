using System;

namespace jjm.one.RabbitMqClientWrapper.util;

/// <summary>
/// This class contains some functions to convert from/to a <see cref="TimeSpan"/> object.
/// </summary>
internal static class TimeSpanConverter
{
    #region internal static extension functions

    /// <summary>
    /// Converts an <see cref="int"/> object to a <see cref="TimeSpan"/>.
    /// </summary>
    /// <param name="milliseconds"></param>
    /// <returns></returns>
    internal static TimeSpan MillisecondsToTimeSpan(this int milliseconds)
    {
        return new TimeSpan(days: 0, hours: 0, minutes: 0, seconds: 0, milliseconds: milliseconds);
    }

    #endregion
}