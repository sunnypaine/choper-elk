# choper-elk

`choper-elk` 是一个“集实现IOC容器与支持WCF多种协议”WCF服务容器的框架。

将你的服务全权交由choper-elk托管，通过配置切换框架对WebHttpBinding、BasicHttpBinding、TCPBinding三种协议的切换和支持。内置的IOC可让你的工程的各个模块解耦，就像Java的程序员使用Spring-Framework一样，添加一系列特性（就像Java的注解）即可完成实例注入。

## 目录

[`一. 工程结构说明`](#一-工程结构说明)

[`二. 加入依赖的动态链接库`](二-加入依赖的动态链接库)

[`三. 快速搭建一个支持REST调用的WCF服务`](三-快速搭建一个支持REST调用的WCF服务)

[`四. 核心API`](四-核心API)

[`五. 更新日志`](五-更新日志)

[`六. 示例程序`](六-示例程序)

## 一. 工程结构说明

Choper.Elk ······································· Elk的WCF服务容器、以及IOC容器。

Choper.Elk.Buckhorn ·························· Elk公共特性、公共异常、公共枚举等。

Choper.Elk.Test.Contract ····················· 测试用。测试工程的WCF接口协议。

Choper.Elk.Test.BLL ··························· 测试用。测试工程的业务逻辑层。

Choper.Elk.Test.DAL ··························· 测试用。测试工程的数据访问层。

Choper.Elk.Test.Model ························ 测试用。测试工程的实体。

## 二. 加入依赖的动态链接库

1. 引用CPJIT.Library.CPJ4net.dll。你也可以从 https://github.com/sunnypaine/cpj4net 上面下载。当然，既然依赖了此工具包，CPJIT.Library.CPJ4net.dll所依赖的一堆包也是需要的。

2. 工程只需要引用上面的CPJIT.Library.CPJ4net.dll就够了，CPJIT.Library.CPJ4net.dll所依赖的那些dll可以不引用到工程中，只需要在可执行文件夹下存在就可以了。除非你的工程中也需要使用到那些包。

## 三. 快速搭建一个支持REST调用的WCF服务

`第一步` 检查Choper.Elk.exe.config文件的配置信息是否如下：

```xml
<?xml version="1.0" encoding="utf-8">
<configuration>
    <startup>
        <supportedRuntime version="v4.0" sku=".NETFramework,Version=v4.5" />
    </startup>

    <runtime>
        <assemblyBinding xmlns="urn:schemas-microsoft-com:asm.v1">
            <probing privatePath="business"/>
        </assemblyBinding>
    </runtime>

    <appSettings>
        <add key="ip" value="127.0.0.1"/> <!--你的服务所在计算机的IP，可修改-->
        <add key="port" value="9999"/> <!--你的服务的端口，可修改-->
        <add key="version" value="V1.0.0.0"/> <!--你的服务版本-->
    </appSettings>
</configuration>
```

`第二步` 检查application.properties文件。

该文件是choper-elk框架的专属配置文件，必须有，如果没有会给你报错。此配置文件的配置信息填写方法和Java的spring-boot的application.properties一样，就是最易理解的键值对，该配置文件的配置信息将与ConfigurationAttribute的ValueAttribute特性配合使用，Elk框架将会自动把这些配置信息注入到Value特性标识的私有变量中，后面会有说明。示例如下：

```ini
mysql.url=jdbc:mysql://127.0.0.1:3306/zdszhzh
mysql.driver-class-name=com.mysql.jdbc.Driver
mysql.username=szhzh
mysql.password=123456

activemq.url=tcp://127.0.0.1:61616
activemq.username=admin
activemq.password=admin
```

`第三步` 检查可执行程序目录下是否存在business文件夹。当然，如果该文件夹不存在，框架将自动创建。该文件夹的作用是存放用户开发的WCF服务业务dll文件。

`第四步` 写一个简单的WCF协议接口。示例如下：

```csharp
//定义WCF协议接口
[ServiceContract(Namespace = "http://www.choper.org")]
public interface IUserContarct
{
  [OperationContract]
  [WebInvoke(Method = "GET",
            UriTemplate = "/User/{id}",
            RequestFormat = WebMessageFormat.Json,
            ResponseFormat = WebMessageFormat.Json)]
  [return: MessageParameter(Name = "User")]
  User QueryUserById(string id);
}

//实现协议服务
[Contract("userContract")]
public class UserContractImpl : IUserContract
{
    public User QueryUserById(string id)
  {
      //TODO
  }
}
```

将你的工程编译后的dll放到business文件夹下，

双击Choper.Elk.exe，服务就成功跑起来了。

访问WCF服务地址：http://127.0.0.1:9999/api/v1.0.0.0/UserContract/User/001 ，就可以访问到QueryUserById这个WCF协议接口了。

## 四. 核心API

### 1. ConfigurationAttribute特性

`说明` 该特性作用于类上。当类上做了该特性标识，则标识该类是一个用于做配置信息的类。

`示例` 

```csharp
[Configuration("activemqConfig")]
public ActivemqConfig
{ }
```

### 2. DataAccessAttribute特性

`说明` 该特性作用于类上。当类上做了该特性标识，则标识该类是一个数据访问层的类。

`示例` 

```csharp
[DataAccess("userDataAccess")]
public class UserDataAccessImpl : IUserDataAccess
{ }
```

### 3. BusinessAttribute特性

`说明` 该特性作用于类上。当类上做了该特性标识，则标识该类是一个业务逻辑层的类。

`示例` 

```csharp
[Business("userBusiness")]
public class UserBusinessImpl : IUserBusiness
{ }
```

### 4. ContractAttribute特性

`说明` 该特性作用于类上。当类上做了该特性标识，则标识该类是一个WCF协议实现的类。

`示例` 

```csharp
[Contract]
public class UserContractImpl : IUserContract
{ }
```

### 5. ValueAttribute特性

`说明` 该特性作用于私有变量上。当私有变量做了该特性标识，则choper-elk框架将自动从application.propertie配置文件中读取配置信息并注入对应的私有变量中。

`注意` 只有当类标识了ConfigurationAttribute特性时，类中标识了ValueAttribute特性的私有变量才会被扫描并赋值。

`示例` 

```csharp
[Configuration("activemqConfig")]
public class ActivemqConfig
{
  [Value(Name = "activemq.url")]
  private string url;
  
  [Value(Name = "activemq.username")]
  private string userName;
  
  [Value(Name = "activemq.password")]
  private string password;
}
```

### 6. BeanAttribute特性

`说明` 该特性作用于方法上。当方法上做了该特性标识，则choper-elk框架将自动执行该方法。

`注意` 标识了该特性的方法必须要有返回值。

`示例` 

```csharp
[Configuration("activemqConfig")]
public class ActivemqConfig
{
  [Bean(Name = "activemqConnectionFactory")]
  public IConnectionFacotry ActiveMQConnectionFacotry()
  {
    ConnectionFactory factory = new ConnectionFactory();
    factory.CientId = "ActiveMQ测试客户端";
    return factory;
  }
}
```

### 7. ParameterAttribute特性

`说明` 该特性作用于方法的参数上。当参数做了该特性标识，则choper-elk框架将自动为该参数注入值。

`示例` 

```csharp
[Configuration("activemqConfig")]
public class AcitvemqConfig
{
  [Bean(Name = "activemqConnectionFactory")]
  public IConnectionFacotry ActiveMQConnectionFacotry()
  {
    ConnectionFactory factory = new ConnectionFactory();
    factory.CientId = "ActiveMQ测试客户端";
    return factory;
  }
  
  [Bean(Name = "activemqConnection")]
  public IConnection ActiveMQConnection([Parameter("activemqConnectionFactory")] ConnectionFactory connectionFactory)
  {
    IConnection connection = null;
    try
    {
      connection = connectionFactory.CreateConnection();
      connection.Start();
    }
    catch(Exception e)
    {
      Console.WriteLine(e.Message);
    }
    return connection;
  }
}
```

### 8. ResourceAttribute特性

`说明` 该特性作用于私有变量上。当私有变量做了该特性标识，则choper-elk框架将会为其自动注入值。

`注意` 只有当类标识了DataAccessAttribute特性、或BusinessAttribute特性、或ContractAttribute特性时，类中的标识了ResourceAttribute特性的私有变量才会被自动注入。

`示例` 

```csharp
[Business("userBusiness")]
public UserBusinessImpl : IUserBusiness
{
	[Resource(Name = "userDataAccess")]
  private IUserDataAccess userDataAccess;
  
  public User FindUserById(string userId)
  {
  	if (string.IsNullOrEmpty(userId))
    {
    	return null;
    }
  	return this.userDataAccess.SelectUserById(userId);
  }
}
```



## 五. 更新日志

2018.05.22_ 首次发布[v1.0.0.0]。



## 六. 示例程序

示例程序见工程中Choper.Elk.Test开头的工程。第一章 [`工程结构说明`](#一-工程结构说明) 有详细说明。
