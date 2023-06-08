using System;
using jjm.one.RabbitMqClientWrapper.types;

namespace jjm.one.RabbitMqClientWrapper.Tests.testtools
{
	public class RmqcTestCoreArgs
	{
        #region public members

        public bool ResultOnReturn { get; set; } = true;
        public Exception? ExceptionOnReturn { get; set; } = null;
        public Message? MessageOnReturn { get; set; } = null;
        public uint? AmountOnReturn { get; set; } = null;

        #endregion

        #region ctor's

        public RmqcTestCoreArgs()
        {
            ;
        }

        #endregion
    }
}

