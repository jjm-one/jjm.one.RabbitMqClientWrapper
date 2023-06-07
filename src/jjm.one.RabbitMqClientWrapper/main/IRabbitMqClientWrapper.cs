using jjm.one.RabbitMqClientWrapper.main.core;

namespace jjm.one.RabbitMqClientWrapper.main
{
    public interface IRabbitMqClientWrapper
    {
        #region public functions

        public bool Reconnect();

        #endregion
    }
}
