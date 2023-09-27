namespace MyCloa.Common.Valid;

/// <summary>
/// 数据校验抽象类
/// </summary>
public abstract class ValidRequestBase : IValidRequest
{
    /// <summary>
    /// 数据校验
    /// </summary>
    /// <returns>成功返回true,失败返回false</returns>
    public ValidResult Valid<T>(T request)
    {
        ExecuteBegin(request);
        var result = Execute(request);
        ExecuteEnd(request, result);

        return result;
    }
    /// <summary>
    /// 完成校验
    /// </summary>
    /// <param name="requestEntity">请求对象</param>
    /// <param name="result">校验结果</param>
    /// <typeparam name="TRequest">请求对象类型</typeparam>
    protected virtual void ExecuteEnd<TRequest>(TRequest requestEntity, ValidResult result)
    {
    }

    /// <summary>
    /// 开始校验事件
    /// </summary>
    /// <param name="requestEntity">请求对象</param>
    /// <typeparam name="TRequest">请求对象类型</typeparam>
    protected virtual void ExecuteBegin<TRequest>(TRequest requestEntity)
    {
    }

    /// <summary>
    /// 执行数据校验
    /// </summary>
    /// <param name="request">请求对象</param>
    /// <typeparam name="TRequest">请求对象类型</typeparam>
    /// <returns>返回校验结果</returns>
    protected abstract ValidResult Execute<TRequest>(TRequest request);
}