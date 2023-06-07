using jjm.one.RabbitMqClientWrapper.main;
using jjm.one.RabbitMqClientWrapper.main.core;
using System;

namespace jjm.one.RabbitMqClientWrapper.types.di
{
    /// <summary>
    /// This is a wrapper clas for the <see cref="bool"/> value for enabeling logging in the <see cref="RMQCCore"/> class.
    /// </summary>
	public class DiSimpleTypeWrappersEnableCoreLogging
	{
		#region public members

        /// <summary>
        /// The flag to enable or disable logging i the <see cref="RMQCCore"/> class.
        /// </summary>
		public bool EnableLogging;

        #endregion

        #region ctor

        /// <summary>
        /// Default constructor for the <see cref="DiSimpleTypeWrappersEnableCoreLogging"/> class.
        /// </summary>
        /// <param name="enableLogging">The flag to enable or disable logging i the <see cref="RMQCCore"/> class.</param>
        public DiSimpleTypeWrappersEnableCoreLogging(bool enableLogging = false)
		{
			EnableLogging = enableLogging;
		}

        #endregion
    }

    /// <summary>
    /// This is a wrapper clas for the <see cref="bool"/> value for enabeling logging in the <see cref="RMQCWrapper"/> class.
    /// </summary>
    public class DiSimpleTypeWrappersEnableWrapperLogging
    {
        #region public members

        /// <summary>
        /// The flag to enable or disable logging i the <see cref="RMQCWrapper"/> class.
        /// </summary>
        public bool EnableLogging;

        #endregion

        #region ctor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="enableLogging">The flag to enable or disable logging i the <see cref="RMQCWrapper"/> class.</param>
        public DiSimpleTypeWrappersEnableWrapperLogging(bool enableLogging = false)
        {
            EnableLogging = enableLogging;
        }

        #endregion
    }
}

