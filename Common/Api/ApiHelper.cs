using MyCloa.Common.Command;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;

namespace MyCloa.Common.Api;

/// <summary>
/// Api服务实现
/// </summary>
public sealed class ApiHelper:IApiHelper
{
    /// <summary>
    /// 服务名称
    /// </summary>
    internal static string ServerName = "";
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="resolve">IOC容器</param>
    public ApiHelper(IResolve resolve, ICommandHelper commandHelper = null)
    {
        _resolve = resolve;
        _commandHelper = commandHelper?? DefaultCommandHelper.Instance;
        
    }
    /// <summary>
    /// IOC容器
    /// </summary>
    private readonly IResolve _resolve;

    private readonly ICommandHelper _commandHelper;

    /// <summary>
    /// 处理请求
    /// </summary>
    /// <param name="requestStringEntity">请求参数</param>
    /// <typeparam name="TCommandData">命令元数据类型</typeparam>
    /// <returns>返回Json格式的字符串结果</returns>
    public async Task<string> Call<TCommandData>(RequestStringEntity requestStringEntity) where TCommandData : CommandData
    {
        if (string.IsNullOrEmpty(requestStringEntity.RequestId))
        {
            requestStringEntity.RequestId = Guid.NewGuid().ToString();
        }
        if (IsRemoteCommand(requestStringEntity.ServiceName))
        {
            return await CallRemote(requestStringEntity);
        }

        var result = Invoke<TCommandData>(requestStringEntity);
        return _resolve.Resolve<IDataSerializer>().Serializer(result);
    }

    /// <summary>
    /// 调用远程命令
    /// </summary>
    /// <param name="requestStringEntity">请求参数</param>
    /// <returns>返回调用成功结果</returns>
    public async Task<string> CallRemote(RequestStringEntity requestStringEntity)
    {
       IRemoteCommand remoteCommand = _resolve.Resolve<IRemoteCommand>();
       return await remoteCommand.Execute(requestStringEntity);
    } 

    /// <summary>
    /// 命令检验，认证和授权校验
    /// </summary>
    /// <param name="requestStringEntity">请求字符串</param>
    /// <typeparam name="TCommandData">命令定义数据</typeparam>
    /// <returns></returns>
    private ResultEntity Invoke<TCommandData>(RequestStringEntity requestStringEntity) where TCommandData:CommandData
    {
        if (!_commandHelper.CommandValid(requestStringEntity.CommandName))
        {
            return ResultEntity.Error(requestStringEntity.RequestId, "接口不存在。");
        }

        if (_commandHelper.ValidAuthentication(requestStringEntity.CommandName))
        {
            if (!ValidAuthentication<TCommandData>(requestStringEntity))
            {
                return ResultEntity.Error(requestStringEntity.RequestId, "你没有权限访问此功能。");;
            }
        }

        var command = ResolveCommandService.Instance.Resolve(requestStringEntity.CommandName);
        command.Resolve = _resolve;

        string result;
        try
        {
            result = command.Run(requestStringEntity);
        }
        catch (BusinessException be)
        {
            return ResultEntity.Error(requestStringEntity.RequestId, be.Message);
        }

        return ResultEntity.Success(requestStringEntity.RequestId, result);
    }
    /// <summary>
    /// 是否远程调用命令
    /// 
    /// </summary>
    /// <param name="serviceName">服务名称</param>
    /// <returns>如果服务名称为空或等于当前服务注册名称，就表示本地调用</returns>
    private bool IsRemoteCommand(string serviceName)
    {
        if (string.IsNullOrEmpty(serviceName))
        {
            return false;
        }

        return serviceName != ServerName;
    }

    /// <summary>
    /// 校验授权
    /// </summary>
    /// <param name="requestStringEntity">请求参数</param>
    /// <typeparam name="TCommandData">命令元数据类型</typeparam>
    /// <returns>已授权返回true,其他返回false</returns>
    private bool ValidAuthentication<TCommandData>(RequestStringEntity requestStringEntity) where TCommandData : CommandData
    {
        var auth = _resolve.Resolve<IAuthentication>();
        if (!auth.IsAuthentication(requestStringEntity))
        {
            return false;
        }

        if (_commandHelper.ValidAuthorization(requestStringEntity.CommandName))
        {
            var authorization = _resolve.Resolve<IAuthorization>();
            if (!authorization.IsAuthorization(requestStringEntity,
                    _commandHelper.GetCommandData<TCommandData>(requestStringEntity.CommandName)))
            {
                return false;
            }
        }

        return true;
    }
}