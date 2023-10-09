using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;
using MyCloa.Common.Util;
using MyCloa.Common.Valid;

namespace MyCloa.Common.Command;

/// <summary>
/// 命令抽象类
/// </summary>
/// <typeparam name="TRequest">请求对象类型</typeparam>
/// <typeparam name="TResult">返回结果类型</typeparam>
public abstract class CommandBase<TRequest, TResult> : ICommand
{
    /// <summary>
    /// IOC容器
    /// </summary>
    private IResolve _resolve;

    /// <summary>
    /// 请求对象
    /// </summary>
    private RequestStringEntity _requestStringEntity;

    /// <summary>
    /// 创建远程请求对象
    /// </summary>
    /// <param name="commandEntity">命令对象</param>
    /// <param name="data">命令数据</param>
    /// <returns>返回远程命令对象</returns>
    private RequestStringEntity CreateRemoteRequestStringEntity(CommandEntity commandEntity, string data)
    {
        var result = _requestStringEntity.Copy();
        result.ServiceName = commandEntity.ServiceName;
        result.CommandName = commandEntity.CommandName;
        result.HttpsSchema = commandEntity.HttpsSchema;
        result.Data = data;
        return result;
    }

    /// <summary>
    /// 调用远程
    /// </summary>
    /// <param name="commandEntity">命令对象</param>
    /// <param name="data">命令数据</param>
    /// <returns>返回远程调用结果</returns>
    protected async Task<string> RunRemoteCommand(CommandEntity commandEntity, string data)
    {
        var requestStringEntity = CreateRemoteRequestStringEntity(commandEntity, data);
        var remoteCommand = _resolve.Resolve<IRemoteCommand>();
        return await remoteCommand.Execute(requestStringEntity);
    }

    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="requestStringEntity">命令对象</param>
    /// <returns>返回执行结果</returns>
    /// <exception cref="BusinessException">参数不正确时，抛出异常</exception>
    public string Run(RequestStringEntity requestStringEntity)
    {
        _requestStringEntity = requestStringEntity;
        string request = requestStringEntity.Data ?? "";
        if (string.IsNullOrEmpty(request))
        {
            throw new BusinessException("输入参数不能为空。");
        }

        var requestEntity = ConvertRequest<TRequest>(request);
        if (requestEntity == null)
        {
            throw BusinessException.Create("字符串转对象出错。", request);
        }

        return Execute(request, requestEntity);
    }

    /// <summary>
    /// 命令执行
    /// </summary>
    /// <param name="request">命令数据</param>
    /// <param name="requestEntity">请求对象</param>
    /// <returns></returns>
    private string Execute(string request, [DisallowNull] TRequest requestEntity)
    {
        ValidRequest(request, requestEntity);

        ExecuteBegin(requestEntity);
        var data = ExecuteCore(requestEntity);
        ExecuteEnd(requestEntity, data);
        return Resolve.Resolve<IDataSerializer>().Serializer(data);
    }

    /// <summary>
    /// 命令数据转换
    /// </summary>
    /// <param name="request">命令数据字符串</param>
    /// <typeparam name="T">命令数据类型</typeparam>
    /// <returns>返回命令强类型对象</returns>
    protected virtual T ConvertRequest<T>(string request)
    {
        if (typeof(T) == typeof(string))
        {
            return (T)(object)request;
        }

        if (request.Trim().StartsWith("{") || request.Trim().StartsWith("["))
        {
            return Resolve.Resolve<IDataSerializer>().Deserialize<T>(request);
        }

        return (T)request.To(typeof(T));
    }


    /// <summary>
    /// 检验请求参数
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <param name="requestEntity">请求参数</param>
    /// <exception cref="BusinessException">失败时，抛出此异常</exception>
    protected virtual void ValidRequest(string request, [DisallowNull] TRequest requestEntity)
    {
        var result = ValidRequest(requestEntity);
        if (!result.Success)
        {
            throw BusinessException.Create(result.Message, default, request);
        }
    }

    /// <summary>
    /// 校验命令请求对象
    /// </summary>
    /// <param name="requestEntity"></param>
    /// <returns></returns>
    protected virtual ValidResult ValidRequest(TRequest requestEntity)
    {
        IValidRequest validRequest = _resolve.Resolve<IValidRequest>();
        return validRequest.Valid(requestEntity);
    }

    /// <summary>
    /// IOC容器显示实现
    /// </summary>
    IResolve ICommand.Resolve
    {
        get => _resolve;
        set => _resolve = value;
    }

    /// <summary>
    /// IOC容器实现
    /// </summary>
    protected IResolve Resolve
    {
        get { return _resolve; }
    }

    /// <summary>
    /// 命令执行完成事件
    /// </summary>
    /// <param name="requestEntity">命令对象</param>
    /// <param name="data"></param>
    protected virtual void ExecuteEnd(TRequest requestEntity, TResult data)
    {
    }

    /// <summary>
    /// 命令执行开始事件
    /// </summary>
    /// <param name="requestEntity">命令对象</param>
    protected virtual void ExecuteBegin(TRequest requestEntity)
    {
    }

    /// <summary>
    /// 命令执行
    /// </summary>
    /// <param name="request">请求对象</param>
    /// <returns>返回结果</returns>
    protected abstract TResult ExecuteCore(TRequest request);
}