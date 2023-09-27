using MyCloa.Common.Command;

namespace MyCloa.Common.Query;

/// <summary>
/// 数据查询
/// </summary>
/// <typeparam name="TRequest">请求参数类型</typeparam>
/// <typeparam name="TResult">返回值类型，用于查询单个记录或多个记录</typeparam>
public abstract class QueryBase<TRequest, TResult> : CommandBase<TRequest,TResult>, IQuery
{
    protected override TResult ExecuteCore(TRequest request)
    {
        RequestBegin(request);
        var data = GetData(request);
        RequestEnd(request, data);
        return data;
    }

    /// <summary>
    /// 实现数据查询
    /// </summary>
    /// <param name="request">查询条件</param>
    /// <returns>返回查询结果</returns>
    protected abstract TResult GetData(TRequest request);

    /// <summary>
    /// 开始查询
    /// </summary>
    /// <param name="request">查询条件</param>
    protected virtual void RequestBegin(TRequest request)
    {
    }

    /// <summary>
    /// 查询完成
    /// </summary>
    /// <param name="request">查询参数</param>
    /// <param name="result">返回结果</param>
    protected virtual void RequestEnd(TRequest request, TResult result)
    {
    }

    /// <summary>
    /// 创建返回结果类型实例
    /// 用于创建默认的返回对象，再由数据查询方法填充相关内容
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回创建对象</returns>
    protected abstract TResult CreateResult(TRequest request);
}