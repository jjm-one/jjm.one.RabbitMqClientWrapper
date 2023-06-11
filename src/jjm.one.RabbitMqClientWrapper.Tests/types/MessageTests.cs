using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.types;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types;

/// <summary>
/// This class contains the unit tests for the <see cref="Message"/> class.
/// </summary>
public class MessageTests
{
    #region tests

    #region ctor tests

    /// <summary>
    /// Tests the default constructor of the <see cref="Message"/> class.
    /// </summary>
    [Fact]
    public void MessageTest_DefaultCtorTest()
    {
        // arrange
        Message m = null!;
        
        try
        {
            // act
            m = new Message();
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
        
        // assert 2
        m.Should().NotBeNull();
    }
    
    /// <summary>
    /// Tests the constructor of the <see cref="Message"/> class with <see langword="null"/> as inputs.
    /// </summary>
    [Fact]
    public void MessageTest_CtorNullTest()
    {
        // arrange
        Message m = null!;
        
        try
        {
            // act
            m = new Message(null);
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
        
        // assert 2
        m.Should().NotBeNull();
    }
    
    /// <summary>
    /// Tests the constructor of the <see cref="Message"/> class.
    /// </summary>
    [Fact]
    public void MessageTest_CtorTest()
    {
        // arrange
        Message m = null!;
        var r = 
            new BasicGetResult(0, false, "", "", 0, null, null);
        
        try
        {
            // act
            m = new Message(r);
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
        
        // assert 2
        m.Should().NotBeNull();
    }

    #endregion

    #region public members tests

    /// <summary>
    /// Tests the getter of the DeliveryTag member.
    /// </summary>
    [Fact]
    public void MessageTest_DeliveryTagGetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, b));
        ulong res = 0;
        
        try
        {
            // act
            res = m.DeliveryTag;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
            
        // assert 2
        res.Should().Be(42);
    }
    
    /// <summary>
    /// Tests the getter of the RoutingKey member.
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeyGetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, b));
        var res = string.Empty;
        
        try
        {
            // act
            res = m.RoutingKey;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
            
        // assert 2
        res.Should().Be("TEST-RK");
    }

    /// <summary>
    /// Tests the setter of the RoutingKey member.
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeySetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX",string.Empty, 69,
                null, b));
        var res = string.Empty;
        
        try
        {
            // act
            m.RoutingKey = "TEST-RK";
            res = m.RoutingKey;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
        
        // assert 2
        res.Should().Be("TEST-RK");
    }
    
    /// <summary>
    /// Tests the getter of the BasicProperties member.
    /// </summary>
    [Fact(Skip = "Not properly implemented Test.")]
    public void MessageTest_BasicPropertiesGetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, b));
        IBasicProperties? res = null!;
        
        try
        {
            // act
            res = m.BasicProperties;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
            
        // assert 2
        res.Should().BeNull();
    }
    
    /// <summary>
    /// Tests the getter of the Body member.
    /// </summary>
    [Fact]
    public void MessageTest_BodyGetSetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, b));
        ReadOnlyMemory<byte>? res = null;
        
        try
        {
            // act 1
            res = m.Body;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
            
        // assert 2
        res.Should().Be(b);
    }
    
    /// <summary>
    /// Tests the setter of the Body member.
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, null));
        ReadOnlyMemory<byte>? res = null;
        
        try
        {
            // act
            m.Body = b;
            res = m.Body;
        }
        catch (Exception exc)
        {
            // assert 1
            Assert.Fail(exc.Message);
        }
            
        // assert 2
        res.Should().Be(b);
    }

    #endregion
    
    #endregion
}