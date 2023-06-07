namespace jjm.one.RabbitMqClientWrapper.types
{
    public class Settings
    {
        #region private mebers

        private string? _hostname;
        private int? _port;
        private string? _username;
        private string? _password;
        private string? _vHost;
        private string? _exchange;
        private string? _queue;

        #endregion

        #region public members

        public string Hostname
        {
            get => _hostname ?? "localhost";
            set => _hostname = value;
        }

        public int Port
        {
            get => _port ?? 5672;
            set => _port = value;
        }

        public string Username
        {
            get => _username ?? "guest";
            set => _username = value;
        }

        public string Password
        {
            get => _password ?? "guest";
            set => _password = value;
        }

        public string VHost
        {
            get => _vHost ?? "/";
            set => _vHost = value;
        }

        public string Exchange
        {
            get => _exchange ?? "amq.direct";
            set => _exchange = value;
        }

        public string Queue
        {
            get => _queue ?? "";
            set => _queue = value;
        }

        #endregion

        #region ctor

        public Settings()
        {
            ;
        }

        public Settings(string? hostname, int? port, string? username, string? password,
            string? vHost, string? exchange, string? queue)
        {
            _hostname = hostname;
            _port = port;
            _username = username;
            _password = password;
            _vHost = vHost;
            _exchange = exchange;
            _queue = queue;
        }

        #endregion
    }
}
