using jjm.one.RabbitMqClientWrapper.types;
using System;

namespace jjm.one.RabbitMqClientWrapper.Tests.types
{
    /// <summary>
    /// This class contains the unit tests for the <see cref="Settings"/> class.
    /// </summary>
    public class SettingsTests
	{
        #region tests

        /// <summary>
        /// Tests the default constructor of the <see cref="Settings"/> class.
        /// </summary>
        [Fact]
        public void SettingsTest_DefaultCtorTest()
        {
            var s = new Settings();

            Assert.Equal("localhost", s.Hostname);
            Assert.Equal(5672, s.Port);
            Assert.Equal("guest", s.Username);
            Assert.Equal("guest", s.Password);
            Assert.Equal("/", s.VHost);
            Assert.Equal("amq.direct", s.Exchange);
            Assert.Equal("", s.Queue);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="Settings"/> class with <see langword="null"/> as inputs.
        /// </summary>
        [Fact]
        public void SettingsTest_CtorNullTest()
        {
            var s = new Settings(null, null, null, null, null, null ,null);

            Assert.Equal("localhost", s.Hostname);
            Assert.Equal(5672, s.Port);
            Assert.Equal("guest", s.Username);
            Assert.Equal("guest", s.Password);
            Assert.Equal("/", s.VHost);
            Assert.Equal("amq.direct", s.Exchange);
            Assert.Equal("", s.Queue);
        }

        /// <summary>
        /// Tests the constructor of the <see cref="Settings"/> class.
        /// </summary>
        [Fact]
        public void SettingsTest_CtorTest()
        {
            var s = new Settings("A", 42, "B", "C", "D", "E", "F");

            Assert.Equal("A", s.Hostname);
            Assert.Equal(42, s.Port);
            Assert.Equal("B", s.Username);
            Assert.Equal("C", s.Password);
            Assert.Equal("D", s.VHost);
            Assert.Equal("E", s.Exchange);
            Assert.Equal("F", s.Queue);
        }

        /// <summary>
        /// Tests the Equals method of the <see cref="Settings"/> class.
        /// </summary>
        [Fact]
        public void SettingsTest_Equals()
        {
            var s1 = new Settings("A", 42, "B", "C", "D", "E", "F");
            var s2 = new Settings("A", 42, "B", "C", "D", "E", "F");
            var s3 = new Settings("a", 69, "b", "c", "d", "e", "f");

            Assert.True(s1.Equals(s2));
            Assert.False(s1.Equals(s3));
        }

        /// <summary>
        /// Tests the GetHashCode method of the <see cref="Settings"/> class.
        /// </summary>
        [Fact]
        public void SettingsTest_Hash()
        {
            var s1 = new Settings("A", 42, "B", "C", "D", "E", "F");
            var s2 = new Settings("A", 42, "B", "C", "D", "E", "F");
            var s3 = new Settings("a", 69, "b", "c", "d", "e", "f");

            var h1 = s1.GetHashCode();
            var h2 = s2.GetHashCode();
            var h3 = s3.GetHashCode();

            Assert.True(h1.Equals(h2));
            Assert.False(h1.Equals(h3));

        }

        #endregion
    }
}

