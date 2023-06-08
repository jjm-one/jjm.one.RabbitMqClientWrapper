using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;

namespace jjm.one.RabbitMqClientWrapper.types.di
{
    /// <summary>
    /// This is a wrapper clas for the <see cref="bool"/> value for enable logging in the <see cref="RmqcCore"/> class.
    /// </summary>
	public class DiSimpleTypeWrappersEnableCoreLogging
	{
		#region public members

        /// <summary>
        /// The flag to enable or disable logging i the <see cref="RmqcCore"/> class.
        /// </summary>
		public readonly bool EnableLogging;

        #endregion

        #region ctor

        /// <summary>
        /// Default constructor for the <see cref="DiSimpleTypeWrappersEnableCoreLogging"/> class.
        /// </summary>
        /// <param name="enableLogging">The flag to enable or disable logging i the <see cref="RmqcCore"/> class.</param>
        public DiSimpleTypeWrappersEnableCoreLogging(bool enableLogging = false)
		{
			EnableLogging = enableLogging;
		}

        #endregion
    }

    /// <summary>
    /// This is a wrapper clas for the <see cref="bool"/> value for enable logging in the <see cref="RmqcWrapper"/> class.
    /// </summary>
    public class DiSimpleTypeWrappersEnableWrapperLogging
    {
        #region public members

        /// <summary>
        /// The flag to enable or disable logging i the <see cref="RmqcWrapper"/> class.
        /// </summary>
        public readonly bool EnableLogging;

        #endregion

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableLogging">The flag to enable or disable logging i the <see cref="RmqcWrapper"/> class.</param>
        public DiSimpleTypeWrappersEnableWrapperLogging(bool enableLogging = false)
        {
            EnableLogging = enableLogging;
        }

        #endregion
    }
}

