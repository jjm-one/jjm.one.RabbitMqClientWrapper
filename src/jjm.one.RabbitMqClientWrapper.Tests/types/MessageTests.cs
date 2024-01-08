using System;
using System.Collections.Generic;
using System.Text;
using jjm.one.RabbitMqClientWrapper.types;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types;

/// <summary>
///     This class contains the unit tests for the <see cref="RmqcMessage" /> class.
/// </summary>
public class MessageTests
{
    #region new tests

    /// <summary>
    ///     Test the RoutingKey property.
    /// </summary>
    [Fact]
    public void TestRoutingKeyProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var newRoutingKey = "newRoutingKey";

        // Act
        rmqcMessage.RoutingKey = newRoutingKey;

        // Assert
        rmqcMessage.RoutingKey.Should().Be(newRoutingKey);
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the BasicProperties property.
    /// </summary>
    [Fact]
    public void TestBasicPropertiesProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var basicPropertiesMock = new Mock<IBasicProperties>();
        basicPropertiesMock.Setup(bp => bp.Headers).Returns(new Dictionary<string, object>());

        // Act
        rmqcMessage.BasicProperties = basicPropertiesMock.Object;

        // Assert
        rmqcMessage.BasicProperties.Should().Be(basicPropertiesMock.Object);
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the Headers property.
    /// </summary>
    [Fact]
    public void TestHeadersProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var headers = new Dictionary<string, object>();

        // Act
        rmqcMessage.Headers = headers;

        // Assert
        rmqcMessage.Headers.Should().BeEquivalentTo(headers);
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the BodyArray property.
    /// </summary>
    [Fact]
    public void TestBodyArrayProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var bodyArray = "test"u8.ToArray();

        // Act
        rmqcMessage.BodyArray = bodyArray;

        // Assert
        rmqcMessage.BodyArray.Should().BeEquivalentTo(bodyArray);
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the BodyString property.
    /// </summary>
    [Fact]
    public void TestBodyStringProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var bodyString = "test";

        // Act
        rmqcMessage.BodyString = bodyString;

        // Assert
        rmqcMessage.BodyString.Should().Be(bodyString);
        rmqcMessage.WasModified.Should().BeTrue();
    }
    
    /// <summary>
    ///     Test the WasModified property.
    /// </summary>
    [Fact]
    public void TestWasModifiedProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.WasModified = true;

        // Assert
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasSaved property.
    /// </summary>
    [Fact]
    public void TestWasSavedProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.WasSaved = true;

        // Assert
        rmqcMessage.WasSaved.Should().BeTrue();
        rmqcMessage.WasModified.Should().BeFalse();
    }

    /// <summary>
    ///     Test the BasicProperties property when the value is different.
    /// </summary>
    [Fact]
    public void TestBasicPropertiesPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var basicPropertiesMock = new Mock<IBasicProperties>();
        basicPropertiesMock.Setup(bp => bp.Headers).Returns(new Dictionary<string, object>());

        // Act
        rmqcMessage.BasicProperties = basicPropertiesMock.Object;

        // Assert
        rmqcMessage.BasicProperties.Should().Be(basicPropertiesMock.Object);
        rmqcMessage.Headers.Should().BeEquivalentTo(new Dictionary<string, object>());
        rmqcMessage.TimestampWhenReceived.Should().BeNull();
        rmqcMessage.TimestampWhenSend.Should().BeNull();
        rmqcMessage.TimestampWhenAcked.Should().BeNull();
        rmqcMessage.TimestampWhenNAcked.Should().BeNull();
        rmqcMessage.WasNAckedWithRequeue.Should().BeFalse();
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the Headers property when the value is different.
    /// </summary>
    [Fact]
    public void TestHeadersPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var headers = new Dictionary<string, object>();
        var basicPropertiesMock = new Mock<IBasicProperties>();
        basicPropertiesMock.Setup(bp => bp.Headers).Returns(new Dictionary<string, object>());
        rmqcMessage.BasicProperties = basicPropertiesMock.Object;

        // Act
        rmqcMessage.Headers = headers;

        // Assert
        rmqcMessage.Headers.Should().BeEquivalentTo(headers);
        rmqcMessage.BasicProperties.Headers.Should().BeEquivalentTo(headers);
        rmqcMessage.TimestampWhenReceived.Should().BeNull();
        rmqcMessage.TimestampWhenSend.Should().BeNull();
        rmqcMessage.TimestampWhenAcked.Should().BeNull();
        rmqcMessage.TimestampWhenNAcked.Should().BeNull();
        rmqcMessage.WasNAckedWithRequeue.Should().BeFalse();
        rmqcMessage.WasModified.Should().BeTrue();
    }
    
    /// <summary>
    ///     Test the BodyArray property when the value is different.
    /// </summary>
    [Fact]
    public void TestBodyArrayPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var bodyArray = Encoding.UTF8.GetBytes("Test");

        // Act
        rmqcMessage.BodyArray = bodyArray;

        // Assert
        rmqcMessage.BodyArray.Should().BeEquivalentTo(bodyArray);
        rmqcMessage.BodyString.Should().Be("Test");
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the BodyString property when the value is different.
    /// </summary>
    [Fact]
    public void TestBodyStringPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var bodyString = "Test";

        // Act
        rmqcMessage.BodyString = bodyString;

        // Assert
        rmqcMessage.BodyString.Should().Be(bodyString);
        rmqcMessage.BodyArray.Should().BeEquivalentTo(Encoding.UTF8.GetBytes(bodyString));
        rmqcMessage.WasModified.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasModified property when the value is different.
    /// </summary>
    [Fact]
    public void TestWasModifiedPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.WasModified = true;

        // Assert
        rmqcMessage.WasModified.Should().BeTrue();
        rmqcMessage.WasSaved.Should().BeFalse();
    }

    /// <summary>
    ///     Test the WasSaved property when the value is different.
    /// </summary>
    [Fact]
    public void TestWasSavedPropertyWhenValueIsDifferent()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.WasSaved = true;

        // Assert
        rmqcMessage.WasSaved.Should().BeTrue();
        rmqcMessage.WasModified.Should().BeFalse();
    }

    /// <summary>
    ///     Test the WasReceived property.
    /// </summary>
    [Fact]
    public void TestWasReceivedProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.TimestampWhenReceived = DateTime.Now;

        // Assert
        rmqcMessage.WasReceived.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasSend property.
    /// </summary>
    [Fact]
    public void TestWasSendProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.TimestampWhenSend = DateTime.Now;

        // Assert
        rmqcMessage.WasSend.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasAcked property.
    /// </summary>
    [Fact]
    public void TestWasAckedProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.TimestampWhenAcked = DateTime.Now;

        // Assert
        rmqcMessage.WasAcked.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasNAcked property.
    /// </summary>
    [Fact]
    public void TestWasNAckedProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.TimestampWhenNAcked = DateTime.Now;

        // Assert
        rmqcMessage.WasNAcked.Should().BeTrue();
    }

    /// <summary>
    ///     Test the WasNAckedWithRequeue property.
    /// </summary>
    [Fact]
    public void TestWasNAckedWithRequeueProperty()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();

        // Act
        rmqcMessage.WasNAckedWithRequeue = true;

        // Assert
        rmqcMessage.WasNAckedWithRequeue.Should().BeTrue();
    }

    /// <summary>
    ///     Test the constructor with a BasicGetResult parameter.
    /// </summary>
    [Fact]
    public void TestConstructorWithBasicGetResultParameter()
    {
        // Arrange
        var basicGetResult = new BasicGetResult(1, false, "exchange", "routingKey", 1, null, new ReadOnlyMemory<byte>());

        // Act
        var rmqcMessage = new RmqcMessage(basicGetResult);

        // Assert
        rmqcMessage.DeliveryTag.Should().Be(1);
        rmqcMessage.Redelivered.Should().BeFalse();
        rmqcMessage.Exchange.Should().Be("exchange");
        rmqcMessage.RoutingKey.Should().Be("routingKey");
        rmqcMessage.MessageCount.Should().Be(1);
        rmqcMessage.BasicProperties.Should().BeNull();
        rmqcMessage.Headers.Should().BeNull();
        rmqcMessage.BodyArray.Should().BeEquivalentTo(Array.Empty<byte>());
        rmqcMessage.WasModified.Should().BeFalse();
        rmqcMessage.WasSaved.Should().BeFalse();
    }

    /// <summary>
    ///     Test the Changed event gets invoked correctly when the RoutingKey property is set to a new value.
    /// </summary>
    [Fact]
    public void TestChangedEventWhenRoutingKeyIsSetToNewValue()
    {
        // Arrange
        var rmqcMessage = new RmqcMessage();
        var wasEventTriggered = false;
        rmqcMessage.Changed += (sender, args) => wasEventTriggered = true;

        // Act
        rmqcMessage.RoutingKey = "NewRoutingKey";

        // Assert
        wasEventTriggered.Should().BeTrue();
    }

    /// <summary>
    ///     Test the default constructor of the RmqcMessage class.
    /// </summary>
    [Fact]
    public void TestDefaultConstructor()
    {
        // Act
        var rmqcMessage = new RmqcMessage();

        // Assert
        rmqcMessage.Should().NotBeNull();
    }

    /// <summary>
    ///     Test the parameterized constructor of the RmqcMessage class.
    /// </summary>
    [Fact]
    public void TestParameterizedConstructor()
    {
        // Arrange
        var basicPropertiesMock = new Mock<IBasicProperties>();
        basicPropertiesMock.Setup(bp => bp.Headers).Returns(new Dictionary<string, object>());

        var basicGetResult = new BasicGetResult(1, false, "TestExchange", "TestRoutingKey", 0, basicPropertiesMock.Object, new ReadOnlyMemory<byte>(Encoding.UTF8.GetBytes("Test message")));

        // Act
        var rmqcMessage = new RmqcMessage(basicGetResult);

        // Assert
        rmqcMessage.Should().NotBeNull();
        rmqcMessage.DeliveryTag.Should().Be(1);
        rmqcMessage.Redelivered.Should().BeFalse();
        rmqcMessage.Exchange.Should().Be("TestExchange");
        rmqcMessage.RoutingKey.Should().Be("TestRoutingKey");
        rmqcMessage.MessageCount.Should().Be(0);
        rmqcMessage.BasicProperties.Should().BeEquivalentTo(basicPropertiesMock.Object);
        rmqcMessage.Headers.Should().BeEquivalentTo(basicPropertiesMock.Object.Headers);
        rmqcMessage.BodyArray.Should().BeEquivalentTo(Encoding.UTF8.GetBytes("Test message"));
    }
    

    
    
    #endregion
    
    #region tests

    #region ctor tests

    /// <summary>
    ///     Tests the default constructor of the <see cref="RmqcMessage" /> class.
    /// </summary>
    [Fact]
    public void MessageTest_DefaultCtorTest()
    {
        // arrange
        RmqcMessage m = null!;

        try
        {
            // act
            m = new RmqcMessage();
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
    ///     Tests the constructor of the <see cref="RmqcMessage" /> class with <see langword="null" /> as inputs.
    /// </summary>
    [Fact]
    public void MessageTest_CtorNullTest()
    {
        // arrange
        RmqcMessage m = null!;

        try
        {
            // act
            m = new RmqcMessage(null);
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
    ///     Tests the constructor of the <see cref="RmqcMessage" /> class.
    /// </summary>
    [Fact]
    public void MessageTest_CtorTest()
    {
        // arrange
        RmqcMessage m = null!;
        var r =
            new BasicGetResult(0, false, "", "", 0, null, null);

        try
        {
            // act
            m = new RmqcMessage(r);
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
    ///     Tests the getter of the DeliveryTag member.
    /// </summary>
    [Fact]
    public void MessageTest_DeliveryTagGetTest()
    {
        // arrange
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.DeliveryTag;

        // assert
        res.Should().Be(42);
    }

    /// <summary>
    ///     Tests the getter of the RoutingKey member.
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeyGetTest()
    {
        // arrange
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.RoutingKey;

        // assert
        res.Should().Be("TEST-RK");
    }

    /// <summary>
    ///     Tests the setter of the RoutingKey member. (Test 1)
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeySetTest1()
    {
        // arrange
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", string.Empty, 69,
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
    ///     Tests the setter of the RoutingKey member. (Test 2)
    /// </summary>
    [Fact]
    public void MessageTest_RoutingKeySetTest2()
    {
        // arrange
        var m = new RmqcMessage
        {
            // act
            RoutingKey = "TEST-RK"
        };

        var res = m.RoutingKey;

        // assert
        res.Should().Be("TEST-RK");
    }

    /// <summary>
    ///     Tests the getter of the BasicProperties member.
    /// </summary>
    [Fact(Skip = "Not properly implemented Test.")]
    public void MessageTest_BasicPropertiesGetTest()
    {
        // arrange
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, new ReadOnlyMemory<byte>()));

        // act
        var res = m.BasicProperties;

        // assert
        res.Should().BeNull();
    }

    /// <summary>
    ///     Tests the setter of the BasicProperties member.
    /// </summary>
    [Fact(Skip = "Not properly implemented Test.")]
    public void MessageTest_BasicPropertiesSetTest()
    {
        // arrange
        IBasicProperties? bP = null;
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
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
    ///     Tests the getter of the Body member.
    /// </summary>
    [Fact]
    public void MessageTest_BodyGetSetTest()
    {
        // arrange
        var b = Array.Empty<byte>();
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, b));
        // act
        var res = m.BodyArray;

        // assert
        res.Should().BeEquivalentTo(b);
    }

    /// <summary>
    ///     Tests the setter of the Body member. (Test 1)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest1()
    {
        // arrange
        var b = Array.Empty<byte>();

        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, null))
        {
            // act
            BodyArray = b
        };

        var res = m.BodyArray;
        // assert
        res.Should().NotBeNull();
        res.Should().BeEquivalentTo(b);
    }

    /// <summary>
    ///     Tests the setter of the Body member. (Test 2)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest2()
    {
        // arrange
        var b = Array.Empty<byte>();
        var m = new RmqcMessage
        {
            // act
            BodyArray = b
        };

        var res = m.BodyArray;

        // assert
        res.Should().NotBeNull();
        res.Should().BeEquivalentTo(b);
    }

    /// <summary>
    ///     Tests the setter of the Body member. (Test 3)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest3()
    {
        // arrange
        var m = new RmqcMessage(
            new BasicGetResult(42, true, "TEST-EX", "TEST-RK", 69,
                null, null))
        {
            // act
            BodyArray = null
        };

        var res = m.BodyArray;

        // assert
        res.Should().BeNull();
    }

    /// <summary>
    ///     Tests the setter of the Body member. (Test 4)
    /// </summary>
    [Fact]
    public void MessageTest_BodySetTest4()
    {
        // arrange
        var m = new RmqcMessage
        {
            // act
            BodyArray = null
        };

        var res = m.BodyArray;

        // assert
        res.Should().BeNull();
    }

    #endregion

    #endregion
}
