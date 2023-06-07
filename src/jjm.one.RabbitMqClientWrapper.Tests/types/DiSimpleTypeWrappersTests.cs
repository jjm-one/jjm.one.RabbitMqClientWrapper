using System;
using jjm.one.RabbitMqClientWrapper.types.di;

namespace jjm.one.RabbitMqClientWrapper.Tests.types
{
    /// <summary>
    /// This class contains the unit tests for the following classes:
    /// - <see cref="DiSimpleTypeWrappersEnableCoreLogging"/>
    /// - <see cref="DiSimpleTypeWrappersEnableWrapperLogging"/>
    /// </summary>
	public class DiSimpleTypeWrappersTests
	{
        #region tests

        #region DiSimpleTypeWrappersEnableCoreLogging tests

        /// <summary>
        /// 1. test of the <see cref="DiSimpleTypeWrappersEnableCoreLogging"/> class.
        /// </summary>
        [Fact]
        public void DiSimpleTypeWrappersEnableCoreLoggingTest1()
        {
            var o1 = new DiSimpleTypeWrappersEnableCoreLogging();
            var o2 = new DiSimpleTypeWrappersEnableCoreLogging(false);
            var o3 = new DiSimpleTypeWrappersEnableCoreLogging(true);

            Assert.False(o1.EnableLogging);
            Assert.False(o2.EnableLogging);
            Assert.True(o3.EnableLogging);
        }

        #endregion

        #region DiSimpleTypeWrappersEnableWrapperLogging tests

        /// <summary>
        /// 1. test of the <see cref="DiSimpleTypeWrappersEnableWrapperLogging"/> class.
        /// </summary>
        [Fact]
        public void DiSimpleTypeWrappersEnableWrapperLogging1()
        {
            var o1 = new DiSimpleTypeWrappersEnableWrapperLogging();
            var o2 = new DiSimpleTypeWrappersEnableWrapperLogging(false);
            var o3 = new DiSimpleTypeWrappersEnableWrapperLogging(true);

            Assert.False(o1.EnableLogging);
            Assert.False(o2.EnableLogging);
            Assert.True(o3.EnableLogging);
        }

        #endregion

        #endregion
    }
}

