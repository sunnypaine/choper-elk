using Choper.Elk.Buckhorn.Atrributes;
using Choper.Elk.Buckhorn.Enums;
using Choper.Elk.Buckhorn.Exceptions;
using Choper.Elk.Model;
using CPJIT.Library.CPJ4net.PropertiesUtil;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Choper.Elk
{
    public class Container
    {
        #region 私有变量
        #endregion


        #region 公共变量
        /// <summary>
        /// Bean容器。
        /// </summary>
        public static IDictionary<string, BeanInfo> BeanContainer = new Dictionary<string, BeanInfo>();

        /// <summary>
        /// application.properties文件配置信息。
        /// </summary>
        public static PropertiesUtil Properties;
        #endregion


        /// <summary>
        /// 启动容器。
        /// </summary>
        public void Start()
        {
            //扫描配置信息
            string application = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "application.properties");
            if (!File.Exists(application))
            {
                throw new FileNotFoundException(string.Format("未找到{0}文件。", application));
            }

            Properties = new PropertiesUtil(application);//创建properties文件解析工具，并解析application.properties

            string business = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "business");
            if (!Directory.Exists(business))
            {
                Directory.CreateDirectory(business);
            }
            string[] dlls = Directory.GetFiles(business, "*.*", SearchOption.AllDirectories);
            if (dlls == null || dlls.Length <= 0)
            {
                return;
            }
            ScanConfigurationAttribute(dlls);
            ScanDataAccessAttribute(dlls);
            ScanBusinessAttribute(dlls);
            ScanContractAttribute(dlls);
            ScanBeanAttribute(dlls);
            ScanResource(dlls);
        }

        /// <summary>
        /// 扫描Configuration特性的类，并创建实例。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanConfigurationAttribute(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        //1.扫描类的Configuration特性
                        //获取属性
                        Attribute attributeConfigurationAttribute = clazz.GetCustomAttribute(typeof(ConfigurationAttribute));
                        if (attributeConfigurationAttribute == null)//如果该类没有Configuration特性
                        {
                            continue;
                        }
                        ConfigurationAttribute configurationAttribute = (ConfigurationAttribute)attributeConfigurationAttribute;
                        string configurationName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(configurationAttribute.Name))//如果设置了ConfigurationAttribute的Name值
                        {
                            configurationName = configurationAttribute.Name;
                        }
                        if (BeanContainer.ContainsKey(configurationName))
                        {
                            throw new BeanCreationException(string.Format("不能创建‘{0}’，已经存在该实例。", configurationName));
                        }

                        //添加实例到容器中
                        object clazzInstance = Activator.CreateInstance(clazz);//创建类的实例
                        BeanContainer.Add(configurationName, new BeanInfo() { Bean = clazzInstance, AtrributeType = AtrributeType.Configuration });
                    }//end : if (clazz.IsClass)
                }//end : foreach (Type clazz in types)
            }//end : foreach (string dll in dlls)
        }

        /// <summary>
        /// 扫描DataAccess特性的类，并创建实例。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanDataAccessAttribute(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        //1.扫描类的DataAccess特性
                        //获取属性
                        Attribute attributeDataAccessAttribute = clazz.GetCustomAttribute(typeof(DataAccessAttribute));
                        if (attributeDataAccessAttribute == null)//如果该类没有DataAccess特性
                        {
                            continue;
                        }
                        DataAccessAttribute dataAccessAttribute = (DataAccessAttribute)attributeDataAccessAttribute;
                        string dataAccessName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(dataAccessAttribute.Name))//如果设置了DataAccessAttribute的Name值
                        {
                            dataAccessName = dataAccessAttribute.Name;
                        }
                        if (BeanContainer.ContainsKey(dataAccessName))
                        {
                            throw new BeanCreationException(string.Format("不能创建‘{0}’，已经存在该实例。", dataAccessName));
                        }

                        //添加实例到容器中
                        object clazzInstance = Activator.CreateInstance(clazz);//创建类的实例
                        BeanContainer.Add(dataAccessName, new BeanInfo() { Bean = clazzInstance, AtrributeType = AtrributeType.DataAccess });
                    }//end : if (clazz.IsClass)
                }//end : foreach (Type clazz in types)
            }//end : foreach (string dll in dlls)
        }

        /// <summary>
        /// 扫描Business特性的类，并创建实例。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanBusinessAttribute(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        //1.扫描类的Business特性
                        //获取属性
                        Attribute attributeBusinessAttribute = clazz.GetCustomAttribute(typeof(BusinessAttribute));
                        if (attributeBusinessAttribute == null)//如果该类没有Business特性
                        {
                            continue;
                        }
                        BusinessAttribute businessAttribute = (BusinessAttribute)attributeBusinessAttribute;
                        string bussinessName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(businessAttribute.Name))//如果设置了BusinessAttribute的Name值
                        {
                            bussinessName = businessAttribute.Name;
                        }
                        if (BeanContainer.ContainsKey(bussinessName))
                        {
                            throw new BeanCreationException(string.Format("不能创建‘{0}’，已经存在该实例。", bussinessName));
                        }

                        //添加实例到容器中
                        object clazzInstance = Activator.CreateInstance(clazz);//创建类的实例
                        BeanContainer.Add(bussinessName, new BeanInfo() { Bean = clazzInstance, AtrributeType = AtrributeType.Business });
                    }//end : if (clazz.IsClass)
                }//end : foreach (Type clazz in types)
            }//end : foreach (string dll in dlls)
        }

        /// <summary>
        /// 扫描Contract特性的类，并创建实例。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanContractAttribute(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        //1.扫描类的Contract特性
                        //获取属性
                        Attribute attributeContractAttribute = clazz.GetCustomAttribute(typeof(ContractAttribute));
                        if (attributeContractAttribute == null)//如果该类没有Contract特性
                        {
                            continue;
                        }
                        ContractAttribute contractAttribute = (ContractAttribute)attributeContractAttribute;
                        string contractName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(contractAttribute.Name))//如果设置了ContractAttribute的Name值
                        {
                            contractName = contractAttribute.Name;
                        }
                        if (BeanContainer.ContainsKey(contractName))
                        {
                            throw new BeanCreationException(string.Format("不能创建‘{0}’，已经存在该实例。", contractName));
                        }

                        //添加实例到容器中
                        object clazzInstance = Activator.CreateInstance(clazz);//创建类的实例
                        BeanContainer.Add(contractName, new BeanInfo() { Bean = clazzInstance, AtrributeType = AtrributeType.Contract });
                    }//end : if (clazz.IsClass)
                }//end : foreach (Type clazz in types)
            }//end : foreach (string dll in dlls)
        }

        /// <summary>
        /// 扫描Bean特性的方法，并创建实例。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanBeanAttribute(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        //1.扫描类的Configuration特性
                        //获取属性
                        Attribute attributeConfigurationAttribute = clazz.GetCustomAttribute(typeof(ConfigurationAttribute));
                        if (attributeConfigurationAttribute == null)//如果该类没有Configuration特性
                        {
                            continue;
                        }
                        ConfigurationAttribute configurationAttribute = (ConfigurationAttribute)attributeConfigurationAttribute;
                        string beanName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(configurationAttribute.Name))
                        {
                            beanName = configurationAttribute.Name;
                        }

                        bool isDefault = true;//是否使用默认配置文件
                        PropertiesUtil _properties = null;
                        string configurationPath = "application.properties";
                        if (!string.IsNullOrWhiteSpace(configurationAttribute.Path))//如果设置了ConfigurationAttribute的Path值
                        {
                            configurationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, configurationAttribute.Path);
                            _properties = new PropertiesUtil(configurationPath);
                            isDefault = false;
                        }

                        //2.扫描字段的Value特性
                        FieldInfo[] fieldInfos = clazz.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        foreach (FieldInfo fieldInfo in fieldInfos)
                        {
                            Attribute attributeValueAttribute = fieldInfo.GetCustomAttribute(typeof(ValueAttribute));
                            if (attributeValueAttribute == null)//如果此字段没有Value特性
                            {
                                continue;
                            }

                            ValueAttribute valueAttribute = (ValueAttribute)attributeValueAttribute;
                            string valueName = fieldInfo.Name;//字段名称
                            if (!string.IsNullOrWhiteSpace(valueName))//如果设置了ValueAttribute的Key值
                            {
                                valueName = valueAttribute.Key;
                            }

                            if (BeanContainer.ContainsKey(beanName))
                            {
                                if (isDefault == false)//如果使用的不是默认配置文件
                                {
                                    fieldInfo.SetValue(BeanContainer[beanName].Bean, Convert.ChangeType(_properties[valueName], fieldInfo.FieldType));
                                }
                                else
                                {
                                    fieldInfo.SetValue(BeanContainer[beanName].Bean, Convert.ChangeType(Properties[valueName], fieldInfo.FieldType));
                                }
                            }
                        }


                        //3.扫描方法的Bean特性
                        MethodInfo[] methods = clazz.GetMethods();
                        foreach (MethodInfo method in methods)
                        {
                            Attribute attributeBeanAttribute = method.GetCustomAttribute(typeof(BeanAttribute));
                            if (attributeBeanAttribute == null)//如果此方法没有Bean特性
                            {
                                continue;
                            }
                            BeanAttribute beanAttribute = (BeanAttribute)attributeBeanAttribute;
                            string methodName = method.Name;//方法名称
                            if (!string.IsNullOrWhiteSpace(beanAttribute.Name))//如果设置了BeanAttribute的Name值
                            {
                                methodName = beanAttribute.Name;
                            }
                            if ("Void".Equals(method.ReturnType.Name))//如果此方法无返回值
                            {
                                throw new BeanCreationException(string.Format("创建名为‘{0}’的bean错误。方法需要返回值。", methodName));
                            }
                            if (BeanContainer.ContainsKey(beanAttribute.Name))//判断单例容器里是否已存在
                            {
                                throw new BeanCreationException(string.Format("创建名为‘{0}’的bean错误。已经存在名为‘{1}’的实例。", methodName, methodName));
                            }


                            //4.扫描方法参数的Parameter特性
                            ParameterInfo[] parameterInfos = method.GetParameters();
                            int parameterCount = parameterInfos.Length;
                            if (parameterCount > 0)//如果有参数
                            {
                                object[] parameterValues = new object[parameterCount];
                                int index = 0;
                                foreach (ParameterInfo parameterInfo in parameterInfos)//设置参数的值
                                {
                                    Attribute attributeParameterAttribute = parameterInfo.GetCustomAttribute(typeof(ParameterAttribute));
                                    string parameterName = parameterInfo.Name;//默认参数的名称
                                    if (attributeParameterAttribute != null)//如果有特性标注
                                    {
                                        ParameterAttribute parameterAttribute = (ParameterAttribute)attributeParameterAttribute;
                                        parameterName = parameterAttribute.Name;
                                    }
                                    if (!BeanContainer.ContainsKey(parameterName))//如果从容器中找不到参数的实例
                                    {
                                        if (parameterInfo.HasDefaultValue)//根据参数是否设置了默认值来决定是否抛出异常
                                        {
                                            parameterValues[index] = parameterInfo.DefaultValue;
                                            continue;
                                        }
                                        throw new BeanNotFoundException(string.Format("找不到指定的实例‘{0}’。", parameterName));
                                    }
                                    parameterValues[index] = BeanContainer[parameterName].Bean;
                                    index++;
                                }

                                if (BeanContainer.ContainsKey(beanName))
                                {
                                    //执行方法，得到方法的返回值，并存入单例容器
                                    object objReturn = method.Invoke(BeanContainer[beanName].Bean, parameterValues);
                                    BeanContainer.Add(methodName, new BeanInfo() { Bean = objReturn, AtrributeType = AtrributeType.Bean });
                                }
                            }
                            else//如果无参数
                            {
                                if (BeanContainer.ContainsKey(beanName))
                                {
                                    //执行方法，得到方法的返回值，并存入单例容器
                                    object objReturn = method.Invoke(BeanContainer[beanName].Bean, null);
                                    BeanContainer.Add(methodName, new BeanInfo() { Bean = objReturn, AtrributeType = AtrributeType.Bean });
                                }
                            }
                        }//end : foreach (MethodInfo method in methods)
                    }//end : if (clazz.IsClass)
                }//end : foreach (Type clazz in types)
            }//end : foreach (string dll in dlls)
        }

        /// <summary>
        /// 标记了Component特性的，或标记了继承Component特性的特性的类，为此类中的标记了Resource特性的成员注入值。
        /// </summary>
        /// <param name="dlls">指定的要被扫描的dll。</param>
        private void ScanResource(string[] dlls)
        {
            foreach (string dll in dlls)
            {
                Assembly assembly = Assembly.LoadFile(dll);
                Type[] types = assembly.GetTypes();
                foreach (Type clazz in types)
                {
                    if (clazz.IsClass)
                    {
                        Attribute attributeComponetAttribute = clazz.GetCustomAttribute(typeof(ComponentAttribute));
                        if (attributeComponetAttribute == null)
                        {
                            continue;
                        }

                        ComponentAttribute componentAttribute = (ComponentAttribute)attributeComponetAttribute;
                        string clazzName = clazz.Name.Substring(0, 1).ToLower() + (clazz.Name.Length > 1 ? clazz.Name.Substring(1) : string.Empty);
                        if (!string.IsNullOrWhiteSpace(componentAttribute.Name))
                        {
                            clazzName = componentAttribute.Name;
                        }

                        FieldInfo[] fieldInfos = clazz.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                        foreach (FieldInfo fieldInfo in fieldInfos)
                        {
                            Attribute attributeResourceAttribute = fieldInfo.GetCustomAttribute(typeof(ResourceAttribute));
                            if (attributeResourceAttribute == null)
                            {
                                continue;
                            }

                            ResourceAttribute resourceAttribute = (ResourceAttribute)attributeResourceAttribute;
                            string resourceName = fieldInfo.Name;
                            if (!string.IsNullOrWhiteSpace(resourceAttribute.Name))
                            {
                                resourceName = resourceAttribute.Name;
                            }
                            if (!BeanContainer.ContainsKey(resourceName))
                            {
                                throw new BeanNotFoundException(string.Format("找不到指定的实例‘{0}’。", resourceName));
                            }

                            if (BeanContainer.ContainsKey(clazzName))
                            {
                                fieldInfo.SetValue(BeanContainer[clazzName].Bean, BeanContainer[resourceName].Bean);
                            }
                        }
                    }
                }
            }
        }
    }
}
