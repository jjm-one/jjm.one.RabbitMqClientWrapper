using System;
using System.Runtime.Intrinsics.Arm;
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
    /// Tests the getter of the RawBasicGetResult member.
    /// </summary>
    [Fact]
    public void MessageTest_RawBasicGetResultGetTest()
    {
        // arrange
        var bgr = new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
            null, new ReadOnlyMemory<byte>());
        var m = new Message(bgr);

        // act
        var res = m.RawBasicGetResult;

        // assert
        res.Should().BeEquivalentTo(bgr);
    }
    
    /// <summary>
    /// Tests the getter of the DeliveryTag member.
    /// </summary>
    [Fact]
    public void MessageTest_DeliveryTagGetTest()
    {
        // arrange
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.DeliveryTag;

        // assert
        res.Should().Be(42);
    }
    
    /// <summary>
    /// Tests the getter of the RoutingKey member.
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeyGetTest()
    {
        // arrange
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.RoutingKey;

        // assert
        res.Should().Be("TEST-RK");
    }

    /// <summary>
    /// Tests the setter of the RoutingKey member. (Test 1)
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeySetTest1()
    {
        // arrange
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX",string.Empty, 69,
                null, new ReadOnlyMemory<byte>()))
        {
            // act
            RoutingKey = "TEST-RK"
        };

        var res = m.RoutingKey;

        // assert
        res.Should().Be("TEST-RK");
    }
    
    /// <summary>
    /// Tests the setter of the RoutingKey member. (Testm2)
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeySetTest2()
    {
        // arrange
        var m = new Message
        {
            // act
            RoutingKey = "TEST-RK"
        };

        var res = m.RoutingKey;

        // assert
        res.Should().Be("TEST-RK");
    }
    
    /// <summary>
    /// Tests the getter of the BasicProperties member.
    /// </summary>
    [Fact(Skip = "Not properly implemented Test.")]
    public void MessageTest_BasicPropertiesGetTest()
    {
        // arrange
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.BasicProperties;

        // assert
        res.Should().BeNull();
    }
    
    /// <summary>
    /// Tests the setter of the BasicProperties member.
    /// </summary>
    [Fact(Skip = "Not properly implemented Test.")]
    public void MessageTest_BasicPropertiesSetTest()
    {
        // arrange
        IBasicProperties? bP = null;
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()))
        {
            // act
            BasicProperties = bP
        };

        var res = m.BasicProperties;

        // assert
        res.Should().Be(bP);
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
        // act
        var res = m.Body;
        
        // assert
        res.Should().Be(b);
    }
    
    /// <summary>
    /// Tests the setter of the Body member. (Test 1)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest1()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, null))
        {
            // act
            Body = b
        };

        var res = m.Body;
        // assert
        
        res.Should().Be(b);
    }

    /// <summary>
    /// Tests the setter of the Body member. (Test 2)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest2()
    {
        // arrange
        var b = new ReadOnlyMemory<byte>();
        var m = new Message
        {
            // act
            Body = b
        };

        var res = m.Body;

        // assert
        res.Should().Be(b);
    }
    
    /// <summary>
    /// Tests the setter of the Body member. (Test 3)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest3()
    {
        // arrange
        ReadOnlyMemory<byte> b = null;
        var m = new Message(
            new BasicGetResult(42, true, "TEST-EX","TEST-RK", 69,
                null, null))
        {
            // act
            Body = b
        };

        var res = m.Body;
        // assert
        
        res.Should().Be(b);
    }
    
    /// <summary>
    /// Tests the setter of the Body member. (Test 4)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest4()
    {
        // arrange
        ReadOnlyMemory<byte> b = null; 
        var m = new Message
        {
            // act
            Body = b
        };

        var res = m.Body;

        // assert
        res.Should().Be(b);
    }
    
    #endregion
    
    #endregion
}