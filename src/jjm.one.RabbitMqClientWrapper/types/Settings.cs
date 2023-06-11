using System.Collections.Generic;

namespace jjm.one.RabbitMqClientWrapper.types
{
    /// <summary>
    /// This class defines the settings for a client connection to a RabbitMQ server.
    /// </summary>
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

        /// <summary>
        /// The hostname of the RabbitMQ server.
        ///
        /// default value: localhost
        /// </summary>
        public string Hostname
        {
            get => _hostname ?? "localhost";
            set => _hostname = value;
        }

        /// <summary>
        /// The port of the RabbitMQ server.
        ///
        /// default value: 5672
        /// </summary>
        public int Port
        {
            get => _port ?? 5672;
            set => _port = value;
        }

        /// <summary>
        /// The username of a RabbitMQ server user.
        ///
        /// default value: guest
        /// </summary>
        public string Username
        {
            get => _username ?? "guest";
            set => _username = value;
        }

        /// <summary>
        /// The password of a RabbitMQ server user.
        ///
        /// default value: guest
        /// </summary>
        public string Password
        {
            get => _password ?? "guest";
            set => _password = value;
        }

        /// <summary>
        /// The vhost at the RabbitMQ server.
        ///
        /// default value: /
        /// </summary>
        public string VHost
        {
            get => _vHost ?? "/";
            set => _vHost = value;
        }

        /// <summary>
        /// The exchange at the RabbitMQ server.
        ///
        /// default value: amq.direct
        /// </summary>
        public string Exchange
        {
            get => _exchange ?? "amq.direct";
            set => _exchange = value;
        }

        /// <summary>
        /// The queue at the RabbitMQ server.
        /// 
        /// default value: ""
        /// </summary>
        public string Queue
        {
            get => _queue ?? "";
            set => _queue = value;
        }

        #endregion

        #region ctor

        /// <summary>
        /// Additional parameterised constructor of the <see cref="Settings"/> class.
        /// </summary>
        /// <param name="hostname">The hostname of the RabbitMQ server.</param>
        /// <param name="port">The port of the RabbitMQ server.</param>
        /// <param name="username">The username of a RabbitMQ server user.</param>
        /// <param name="password">The password of a RabbitMQ server user.</param>
        /// <param name="vHost">The vhost at the RabbitMQ server.</param>
        /// <param name="exchange">The exchange at the RabbitMQ server.</param>
        /// <param name="queue">The queue at the RabbitMQ server.</param>
        public Settings(string? hostname = null , int? port = null, string? username = null, string? password = null,
            string? vHost = null, string? exchange = null, string? queue = null)
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

        #region public overide

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Settings"/> object.
        /// </summary>
        /// <param name="obj">The specified object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object, otherwise, <see langword="false"/>.</returns>
        public override bool Equals(object? obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }
            else
            {
                var s = (Settings)obj;

                var res = true;

                res &= Hostname.Equals(s.Hostname);
                res &= Port.Equals(s.Port);
                res &= Username.Equals(s.Username);
                res &= Password.Equals(s.Password);
                res &= VHost.Equals(s.VHost);
                res &= Exchange.Equals(s.Exchange);
                res &= Queue.Equals(s.Queue);
                
                return res;
            }
        }

        /// <summary>
        /// Serves as the default hash function for the <see cref="Settings"/> class.
        /// </summary>
        /// <returns>A hash code for the current <see cref="Settings"/> object.</returns>
        public override int GetHashCode()
        {
            var hashCode = 655511358;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Hostname);
            hashCode = hashCode * -1521134295 + Port.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Username);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Password);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(VHost);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Exchange);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Queue);
            return hashCode;
        }

        #endregion
    }
}
