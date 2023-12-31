# 简介
mycola 是一个使用 .Net Core 6.0 实现的一个 CQRS 基础框架。对外提供统一的命令执行接口，同时支持数据查询命令和数据执行命令，在注册中心的支持下，还支持远程命令调用。

同时还扩展了用户认证和授权验证，方便进行集成；所有命令都是支持强类型，简化应用开发，更易于上手。只要关注于业务，即命令，所有功能都由命令组成。

# 集成

通过VS建立一个Api项目，如名称为：ApiServerSample，按如下步骤进行修改：

1. 建立IOC服务

    ```csharp
    /// <summary>
    /// Ioc服务实现
    /// </summary>
    public class ResolveService : ResolveBase
    {
        private IServiceProvider serviceCollection;
        public ResolveService(IServiceProvider service) : base(default)
        {
            this.serviceCollection = service;
        }
        public override T Resolve<T>()
        {
            return this.serviceCollection.GetService<T>();
        }
    }
    ```
1. 建立命令
    
    ```csharp
    [Command(Name = "Hello", RequestAuthentication = false, RequestAuthorization = false)]
    public class HelloCommand : CommandBase<string, string>
    {
        protected override string ExecuteCore(string request)
        {
            return "Hello " + request;
        }
    }
    ```

1. 调整Program.cs

    ```csharp
    // Add services to the container.
    //1、注册服务
    builder.Services.AddApiHelp(new ApiHelpOption()
    {
        ResolveType = typeof(ResolveService),
        ServerName = "ServerName"
    });
    
    builder.Services.AddControllers();
    
    var app = builder.Build();
    
    // Configure the HTTP request pipeline.
    
    //2、扫描命令
    builder.Services.ScanCommand(new List<Assembly> { typeof(HelloCommand).Assembly });

    ```
    
1. 建立控制器

    ```csharp
    [ApiController]
    [Route("/api")]
    public class CloaController : Controller
    {
        private IApiHelper _helper;
        public CloaController(IApiHelper helper)
        {
            _helper = helper;
        }
        [Route("invoke")]
        [HttpPost]
        public async Task<string> Invoke(RequestStringEntity requestStringEntity)
        {
            return await _helper.Call<CommandData>(requestStringEntity);
        }
    }
    ```
    
1. 运行Api程序
    此处运行时，程序在端口5052,本地访问地址为：http://localhost:5052
    
1. 测试

在ApiPost软件中进行测试，访问地址为：http://localhost:5052/api/invoke
输入参数：

```json
{
    "RequestId":"",
    "CommandName":"Hello",
    "ServiceName":"",
    "Data":"张三"
}
```

输出结果：

```json
{
	"Data": "\"Hello 张三\"",
	"Result": true,
	"ErrorMessage": "",
	"ErrorCode": "",
	"RequestId": "94a61200-ca34-4e56-a9df-a851b7056159"
}
```

输入结果中，包括返回数据项data和RequestId分布式跟踪id

详细的源码，请参考：[源码示例](https://github.com/usteinfo/mycola/tree/main/Sample)