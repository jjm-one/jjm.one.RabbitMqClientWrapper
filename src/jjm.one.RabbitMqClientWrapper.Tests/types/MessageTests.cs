using System;
using FluentAssertions;
using jjm.one.RabbitMqClientWrapper.types;
using RabbitMQ.Client;

namespace jjm.one.RabbitMqClientWrapper.Tests.types;

public class MessageTests
{
    #region tests

    #region ctor tests

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
    
    [Fact]
    public void MessageTest_CtorTest()
    {
        // arrange
        Message m = null!;
        BasicGetResult r = 
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

    #endregion
}