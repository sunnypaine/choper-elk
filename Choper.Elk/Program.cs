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
        private static string IP = ConfigurationManager.AppSettings["ip"];

        /// <summary>
        /// wcf服务端口
        /// </summary>
        private static int port = Convert.ToInt32(ConfigurationManager.AppSettings["port"]);

        /// <summary>
        /// api版本
        /// </summary>
        private static string version = ConfigurationManager.AppSettings["version"];


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
                Console.ReadKey();
                return;
            }

            //if (File.Exists("WcfConfig.xml"))
            //{
            //    string xml = File.ReadAllText("WcfConfig.xml");
            //    WcfConfig config = XmlSerializeUtil.XmlStringToObject<WcfConfig>(xml);
            //}
            

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
                                                    si.Host = new ServiceHost(Container.BeanContainer[beanName].Bean);
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

                ServiceEndpoint se = kv.Value.Host.AddServiceEndpoint(kv.Value.ContractType, new WebHttpBinding()
                {
                    CrossDomainScriptAccessEnabled = true,
                    MaxReceivedMessageSize = 2147483647,
                    ReaderQuotas = XmlDictionaryReaderQuotas.Max
                },
                string.Format("http://{0}:{1}/api/{2}/", IP, port, version) + kv.Value.ContractName.Substring(1));

                se.Behaviors.Add(new WebHttpBehavior()
                {
                    HelpEnabled = true,
                    DefaultOutgoingRequestFormat = WebMessageFormat.Json,
                    DefaultOutgoingResponseFormat = WebMessageFormat.Json,
                    DefaultBodyStyle = WebMessageBodyStyle.Wrapped
                });

                //设置默认服务实例及运行线程
                ServiceBehaviorAttribute serviceBehavior = kv.Value.Host.Description.Behaviors.Find<ServiceBehaviorAttribute>();
                if (null == serviceBehavior)
                {
                    serviceBehavior = new ServiceBehaviorAttribute { InstanceContextMode = InstanceContextMode.Single, UseSynchronizationContext = false, ConcurrencyMode = ConcurrencyMode.Multiple };
                    kv.Value.Host.Description.Behaviors.Add(serviceBehavior);
                }
                serviceBehavior.InstanceContextMode = InstanceContextMode.Single;
                serviceBehavior.UseSynchronizationContext = false;
                serviceBehavior.ConcurrencyMode = ConcurrencyMode.Multiple;

                kv.Value.Host.Description.Behaviors.Add(new AspNetCompatibilityRequirementsAttribute
                {
                    RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed
                });

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
