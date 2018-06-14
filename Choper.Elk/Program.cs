using Choper.Elk.Buckhorn.Atrributes;
using Choper.Elk.Model;
using CPJIT.Library.CPJ4net.XmlUtil;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Choper.Elk
{
    class Program
    {
        /// <summary>
        /// wcf信息。
        /// </summary>
        private static IDictionary<string, ServiceInfo> wcfServers = new Dictionary<string, ServiceInfo>();

        /// <summary>
        /// wcf服务IP
        /// </summary>
        private static readonly string ip = ConfigurationManager.AppSettings["ip"];

        /// <summary>
        /// wcf服务端口
        /// </summary>
        private static readonly int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

        /// <summary>
        /// api版本
        /// </summary>
        private static readonly string version = ConfigurationManager.AppSettings["version"];


        /// <summary>
        /// 程序入口。
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            //启动容器
            Container container = ContainerFactory.CreateContainer();
            try
            {
                container.Start();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("按任意键退出。");
                Console.ReadKey();
                return;
            }

            WcfConfig config = null;
            if (!File.Exists("WcfConfig.xml"))
            {
                Console.WriteLine("找不到指定的文件WcfConfig.xml。");
                return;
            }
            try
            {
                string xml = File.ReadAllText("WcfConfig.xml");
                config = XmlSerializeUtil.XmlStringToObject<WcfConfig>(xml);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("按任意键退出。");
                Console.ReadKey();
                return;
            }

            if (config == null)
            {
                Console.WriteLine("WcfConfig.xml内容不合法。");
                return;
            }
            if (string.IsNullOrWhiteSpace(config.BindingType))
            {
                Console.WriteLine("WcfConfig.xml的BindingType节点的值不合法。");
                return;
            }
            if (!"WebHttpBinding".Equals(config.BindingType)
                && !"BasicHttpBinding".Equals(config.BindingType)
                && !"NetTcpBinding".Equals(config.BindingType))
            {
                Console.WriteLine("WcfConfig.xml的BindingType节点的值不合法。");
                return;
            }


            #region 读取dll中的服务
            //从dll文件中获取服务
            string pathBusiness = @"./business/";
            string[] files = Directory.GetFiles(pathBusiness, "*.dll");
            foreach (string file in files)
            {
                try
                {
                    Assembly assembly = Assembly.LoadFrom(file);//加载dll
                    if (assembly != null)
                    {
                        Type[] classes = assembly.GetTypes();//从dll中获取class
                        foreach (Type type in classes)//遍历classes中实现了接口的class
                        {
                            if (type.IsClass && !type.IsAbstract)
                            {
                                Type[] interfaces = type.GetInterfaces();
                                foreach (Type t in interfaces)//遍历interfaces中有ServiceContract特性的interface
                                {
                                    object[] attrsContract = t.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                                    if (attrsContract != null && attrsContract.Length > 0)
                                    {
                                        ServiceInfo si = new ServiceInfo
                                        {
                                            ServiceName = type.Name,
                                            ServiceDll = type.Assembly.ManifestModule.Name,
                                            ServiceType = type,
                                            ContractType = t,
                                            ContractName = t.Name,
                                            ContractDll = t.Assembly.ManifestModule.Name,
                                        };
                                        try
                                        {
                                            Attribute attributeContractAttribute = type.GetCustomAttribute(typeof(ContractAttribute));
                                            if (attributeContractAttribute == null)
                                            {
                                                //si.Host = new ServiceHost(type);
                                                continue;
                                            }
                                            else
                                            {
                                                ContractAttribute contractAttribute = (ContractAttribute)attributeContractAttribute;
                                                string beanName = type.Name.Substring(0, 1).ToLower() + (type.Name.Length > 1 ? type.Name.Substring(1) : string.Empty);
                                                if (!string.IsNullOrWhiteSpace(contractAttribute.Name))
                                                {
                                                    beanName = contractAttribute.Name;
                                                }

                                                if (Container.BeanContainer.ContainsKey(beanName))
                                                {
                                                    if ("BasicHttpBinding".Equals(config.BindingType))//当协议类型为BasicHttpBinding时，创建Host实例需指定基地址。
                                                    {
                                                        Uri baseAddress = new Uri(string.Format("http://{0}:{1}/api/{2}/", ip, port, version) + t.Name.Substring(1));
                                                        si.Host = new ServiceHost(Container.BeanContainer[beanName].Bean, baseAddress);
                                                    }
                                                    else
                                                    {
                                                        si.Host = new ServiceHost(Container.BeanContainer[beanName].Bean);
                                                    }
                                                }
                                                else
                                                {
                                                    continue;
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            continue;
                                        }
                                        wcfServers.Add(si.ServiceName, si);
                                    }

                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }
            #endregion


            #region 设置Host
            foreach (KeyValuePair<string, ServiceInfo> kv in wcfServers)
            {
                kv.Value.Host.Opened += Host_Opened;
                kv.Value.Host.Closed += Host_Closed;

                if ("WebHttpBinding".Equals(config.BindingType))
                {
                    ServiceEndpoint se = kv.Value.Host.AddServiceEndpoint(kv.Value.ContractType, new WebHttpBinding()
                    {
                        CrossDomainScriptAccessEnabled = config.WebHttpBinding.CrossDomainScriptAccessEnabled,
                        MaxReceivedMessageSize = config.WebHttpBinding.MaxReceivedMessageSize,
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max
                    },
                    string.Format("http://{0}:{1}/api/{2}/", ip, port, version) + kv.Value.ContractName.Substring(1));

                    se.Behaviors.Add(new WebHttpBehavior()
                    {
                        HelpEnabled = true,
                        DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                        DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                        DefaultBodyStyle = WebMessageBodyStyle.Wrapped
                    });

                    kv.Value.Host.Description.Behaviors.Add(new AspNetCompatibilityRequirementsAttribute
                    {
                        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed
                    });
                }
                else if ("BasicHttpBinding".Equals(config.BindingType))
                {
                    ServiceEndpoint se = kv.Value.Host.AddServiceEndpoint(kv.Value.ContractType, new BasicHttpBinding()
                    {
                        AllowCookies = config.BasicHttpBinding.AllowCookies,
                        CloseTimeout = new TimeSpan(config.BasicHttpBinding.CloseTimeout * 10000000),
                        OpenTimeout = new TimeSpan(config.BasicHttpBinding.OpenTimeout * 10000000),
                        SendTimeout = new TimeSpan(config.BasicHttpBinding.SendTimeout * 10000000),
                        ReceiveTimeout = new TimeSpan(config.BasicHttpBinding.ReceiveTimeout * 10000000),
                        BypassProxyOnLocal = config.BasicHttpBinding.BypassProxyOnLocal,
                        HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                        MaxBufferPoolSize = config.BasicHttpBinding.MaxBufferPoolSize,
                        MaxReceivedMessageSize = config.BasicHttpBinding.MaxReceivedMessageSize,
                        MessageEncoding = WSMessageEncoding.Text,
                        TextEncoding = Encoding.GetEncoding(config.BasicHttpBinding.TextEncoding),
                        UseDefaultWebProxy = config.BasicHttpBinding.UseDefaultWebProxy,
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max
                    },
                    string.Format("http://{0}:{1}/api/{2}/", ip, port, version) + kv.Value.ContractName.Substring(1));
                }
                else if ("NetTcpBinding".Equals(config.BindingType))
                {
                    ServiceEndpoint se = kv.Value.Host.AddServiceEndpoint(kv.Value.ContractType, new NetTcpBinding(SecurityMode.None)
                    {
                        CloseTimeout = new TimeSpan(config.NetTcpBinding.CloseTimeout * 10000000),
                        OpenTimeout = new TimeSpan(config.NetTcpBinding.OpenTimeout * 10000000),
                        SendTimeout = new TimeSpan(config.NetTcpBinding.SendTimeout * 10000000),
                        ReceiveTimeout = new TimeSpan(config.NetTcpBinding.ReceiveTimeout * 10000000),
                        TransferMode = TransferMode.StreamedResponse,
                        TransactionProtocol = TransactionProtocol.OleTransactions,
                        TransactionFlow = config.NetTcpBinding.TransactionFlow,
                        HostNameComparisonMode = HostNameComparisonMode.StrongWildcard,
                        MaxBufferPoolSize = config.NetTcpBinding.MaxBufferPoolSize,
                        MaxReceivedMessageSize = config.NetTcpBinding.MaxReceivedMessageSize,
                        MaxBufferSize = config.NetTcpBinding.MaxBufferSize,
                        ListenBacklog = config.NetTcpBinding.ListenBacklog,
                        MaxConnections = config.NetTcpBinding.MaxConnections,
                        ReaderQuotas = XmlDictionaryReaderQuotas.Max
                    },
                    string.Format("net.tcp://{0}:{1}/api/{2}/", ip, port, version) + kv.Value.ContractName.Substring(1));
                }

                //设置默认服务实例及运行线程
                ServiceBehaviorAttribute serviceBehavior = kv.Value.Host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                if (null == serviceBehavior)
                {
                    serviceBehavior = new ServiceBehaviorAttribute
                    {
                        InstanceContextMode = InstanceContextMode.Single,
                        UseSynchronizationContext = false,
                        ConcurrencyMode = ConcurrencyMode.Multiple
                    };
                    kv.Value.Host.Description.Behaviors.Add(serviceBehavior);
                }
                serviceBehavior.InstanceContextMode = InstanceContextMode.Single;
                serviceBehavior.UseSynchronizationContext = false;
                serviceBehavior.ConcurrencyMode = ConcurrencyMode.Multiple;

                ServiceThrottlingBehavior throttle = kv.Value.Host.Description.Behaviors.Find<ServiceThrottlingBehavior>();
                if (throttle == null)
                {
                    throttle = new ServiceThrottlingBehavior();
                    throttle.MaxConcurrentCalls = 2147483647;
                    throttle.MaxConcurrentSessions = 2147483647;
                    throttle.MaxConcurrentInstances = 2147483647;
                    kv.Value.Host.Description.Behaviors.Add(throttle);
                }

                kv.Value.Host.Open();
            }

            Console.WriteLine("按下任意键退出服务！");
            Console.ReadKey();

            foreach (KeyValuePair<string, ServiceInfo> kv in wcfServers)
            {
                if (kv.Value.Host.State == CommunicationState.Opened)
                {
                    kv.Value.Host.Close();
                }
            }
            #endregion

            Console.WriteLine("正在退出...");
            System.Threading.Thread.Sleep(1000);
        }

        static void Host_Opened(object sender, EventArgs e)
        {
            ServiceHost host = sender as ServiceHost;
            Console.WriteLine("服务【" + host.Description.Endpoints[0].Address.ToString() + "】已经启动！");
        }

        static void Host_Closed(object sender, EventArgs e)
        {
            ServiceHost host = sender as ServiceHost;
            Console.WriteLine("服务【" + host.Description.Endpoints[0].Address.ToString() + "】已经停止！");
        }
    }
}
