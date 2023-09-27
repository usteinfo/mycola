namespace MyCloa.Common.Query;

/// <summary>
/// 分页汇总数据抽象类
/// </summary>
/// <typeparam name="TRequest">请求对象类型</typeparam>
/// <typeparam name="TResult">分页数据类型</typeparam>
/// <typeparam name="TData">分页数据明细类型</typeparam>
/// <typeparam name="TSummary">汇总数据类型</typeparam>
public abstract class PageQuerySummaryInnerBase<TRequest, TResult,TData,TSummary>:PageQueryInnerBase<TRequest, TResult,TData>
    where TRequest:PageRequest
    where TResult:PageResult<TData,TSummary>
{
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回查询结果</returns>
    protected override TResult GetData(TRequest request)
    {
        var result = base.GetData(request);
        if (request.IncludeSummary)
        {
            QuerySummaryBegin(request);
            var data = GetSummaryData(request);
            QuerySummaryEnd(request, data);
            result.Summary = data;
        }
        return result;
    }

    /// <summary>
    /// 查询汇总信息
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    protected abstract TSummary GetSummaryData(PageRequest request);

    /// <summary>
    /// 查询汇总数据开始事件
    /// </summary>
    /// <param name="request">请求对象</param>
    protected virtual void QuerySummaryBegin(PageRequest request)
    {
    }

    /// <summary>
    /// 查询汇总数据结束事件
    /// </summary>
    /// <param name="request">请求对象</param>
    /// <param name="result">结果对象</param>
    protected virtual void QuerySummaryEnd(PageRequest request, TSummary result)
    {
    }
}