using System;
namespace jjm.one.RabbitMqClientWrapper.types
{
	public class DiSimpleTypeWrappersEnableCoreLogging
	{
		#region public members

		public bool EnableLogging;

        #endregion

        #region ctor

        public DiSimpleTypeWrappersEnableCoreLogging(bool enableLogging = false)
		{
			EnableLogging = enableLogging;
		}

        #endregion
    }

    public class DiSimpleTypeWrappersEnableWrapperLogging
    {
        #region public members

        public bool EnableLogging;

        #endregion

        #region ctor

        public DiSimpleTypeWrappersEnableWrapperLogging(bool enableLogging = false)
        {
            EnableLogging = enableLogging;
        }

        #endregion
    }
}

