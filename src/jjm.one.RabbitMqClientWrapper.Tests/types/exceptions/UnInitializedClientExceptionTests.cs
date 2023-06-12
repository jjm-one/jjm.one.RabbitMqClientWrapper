using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.types.exceptions;

namespace jjm.one.RabbitMqClientWrapper.Tests.types.exceptions;

/// <summary>
/// This class contains the unit tests for the <see cref="UnInitializedClientException"/> class.
/// </summary>
public class UnInitializedClientExceptionTests
{
    #region tests

    #region ctor tests

    /// <summary>
    /// Tests the default constructor of the <see cref="UnInitializedClientException"/> class.
    /// </summary>
    [Fact]
    public void UnInitializedClientExceptionTest_DefaultCtorTest()
    {
        // arrange + act
        var e = new UnInitializedClientException();
        
        // assert
        e.Message.Should().Be("Client must be initialized and connected to perform this operation!");
    }
    
    /// <summary>
    /// Tests the constructor of the <see cref="UnInitializedClientException"/> class.
    /// </summary>
    [Fact]
    public void UnInitializedClientExceptionTest_CtorTest()
    {
        // arrange + act
        var e = new UnInitializedClientException("TEST");
        
        // assert
        e.Message.Should().Be("Client must be initialized and connected to perform the TEST operation!");
    }

    #endregion

    #endregion
}