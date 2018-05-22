using Apache.NMS;
using Apache.NMS.ActiveMQ;
using Choper.Elk.Buckhorn.Atrributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk.Test.BLL.Config
{
    [Configuration("customConfig")]
    public class ActivemqConfig
    {
        [Value("activemq.server.ip")]
        private string serverIP;

        [Value("activemq.server.port")]
        private int serverPort;

        [Value("activemq.server.username")]
        private string serverUserName;

        [Value("activemq.server.password")]
        private string serverPassword;

        [Value("activemq.clientid.ip")]
        private string clientIP;

        [Value("activemq.clientid.name")]
        private string clientName;

        [Value("activemq.clientid.type")]
        private int clientType;

        [Value("activemq.clientid.identity")]
        private string clientIdentity;


        [Bean(Name = "activemqConnectionFactory")]
        public IConnectionFactory ActivemqConnectionFactory()
        {
            ConnectionFactory factory = new ConnectionFactory(string.Format("failover:tcp://{0}:{1}", serverIP, serverPort));
            return factory;
        }

        [Bean(Name = "activemqConnection")]
        public IConnection ActivemqConnection([Parameter("activemqConnectionFactory")] ConnectionFactory connectionFactory)
        {
            IConnection connection = null;
            try
            {
                if (string.IsNullOrWhiteSpace(serverUserName) || string.IsNullOrWhiteSpace(serverPassword))
                {
                    connection = connectionFactory.CreateConnection();
                }
                else
                {
                    connection = connectionFactory.CreateConnection(serverUserName, serverPassword);
                }
                connection.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return connection;
        }
    }
}
