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
        /// Tests the constructor of the <see cref="DiSimpleTypeWrappersEnableCoreLogging"/> class.
        /// </summary>
        [Fact]
        public void DiSimpleTypeWrappersEnableCoreLogging_CtorTest()
        {
            var o1 = new DiSimpleTypeWrappersEnableCoreLogging();
            var o2 = new DiSimpleTypeWrappersEnableCoreLogging(true);

            Assert.False(o1.EnableLogging);
            Assert.True(o2.EnableLogging);
        }

        #endregion

        #region DiSimpleTypeWrappersEnableWrapperLogging tests

        /// <summary>
        /// Tests the constructor of the <see cref="DiSimpleTypeWrappersEnableWrapperLogging"/> class.
        /// </summary>
        [Fact]
        public void DiSimpleTypeWrappersEnableWrapperLogging_CtorTest()
        {
            var o1 = new DiSimpleTypeWrappersEnableWrapperLogging();
            var o2 = new DiSimpleTypeWrappersEnableWrapperLogging(true);

            Assert.False(o1.EnableLogging);
            Assert.True(o2.EnableLogging);
        }

        #endregion

        #endregion
    }
}

