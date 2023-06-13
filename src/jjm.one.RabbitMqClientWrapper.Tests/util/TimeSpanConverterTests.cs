using System;
using System.Collections.Generic;
using System.ComponentModel;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.util;

namespace jjm.one.RabbitMqClientWrapper.Tests.util;

/// <summary>
/// This class contains the unit tests for the <see cref="TimeSpanConverter"/> class.
/// </summary>
public class TimeSpanConverterTests
{
    #region test data

    /// <summary>
    /// Test Data for the <see cref="TimeSpanConverterTest_MillisecondsToTimeSpanTest"/> test.
    /// </summary>
    /// <returns></returns>
    public static IEnumerable<object[]> MillisecondsToTimeSpanTestData()
    {
        yield return new object[] { 0, new TimeSpan(0, 0, 0, 0, 0) };
        yield return new object[] { 1, new TimeSpan(0, 0, 0, 0, 1) };
        yield return new object[] { 1001, new TimeSpan(0, 0, 0, 1, 1) };
        yield return new object[] { 240002, new TimeSpan(0, 0, 4, 0, 2) };
        yield return new object[] { 14401000, new TimeSpan(0, 4, 0, 1, 0) };
        yield return new object[] { 86402001, new TimeSpan(1, 0, 0, 2, 1) };
    }

    #endregion
    
    #region tests

    /// <summary>
    /// Tests the MillisecondsToTimeSpan method of the <see cref="TimeSpanConverter"/> class.
    /// </summary>
    [Theory]
    [MemberData(nameof(MillisecondsToTimeSpanTestData))]
    public void TimeSpanConverterTest_MillisecondsToTimeSpanTest(int a, TimeSpan expected)
    {
        // arrange

        // act 
        var res = a.MillisecondsToTimeSpan();
        
        // assert
        res.Should().Be(expected);
    }

    #endregion
}