using System.Collections.ObjectModel;

namespace MyCloa.Common.Query;
/// <summary>
/// 分页数据查询命令
/// </summary>
/// <typeparam name="TRequest">请求对象类型</typeparam>
/// <typeparam name="TResult">结果类型</typeparam>
/// <typeparam name="TData">结果数据类型</typeparam>
public abstract class PageQueryInnerBase<TRequest, TResult, TData> : QueryBase<TRequest, TResult>
    where TRequest : PageRequest
    where TResult : PageResult<TData>
{
    /// <summary>
    /// 查询数据
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回查询结果</returns>
    protected override TResult GetData(TRequest request)
    {
        var result = CreateResult(request);
        var totalCount = GetTotalCount(request);
        if (totalCount > 0)
        {
            result.TotalCount = totalCount;
            request.PageIndex = result.PageIndex;            
            QueryBegin(request);
            var data = GetDataList(request);
            QueryEnd(request, data);
            result.PageData = data;
        }
        else
        {
            result.PageData = new ReadOnlyCollection<TData>(new List<TData>());
            result.TotalCount = 0;
        }
        return result;
    }

    /// <summary>
    /// 获取列表数据
    /// </summary>
    /// <param name="request">页面请求参数</param>
    /// <returns>返回查询结果</returns>
    protected abstract List<TData> GetDataList(PageRequest request);

    /// <summary>
    /// 查询开始HOOK
    /// </summary>
    /// <param name="request">页面请求参数</param>
    protected virtual void QueryBegin(PageRequest request)
    {
    }

    /// <summary>
    /// 查询完成HOOK
    /// </summary>
    /// <param name="request">页面请求参数</param>
    /// <param name="result">查询结果</param>
    protected virtual void QueryEnd(PageRequest request, List<TData> result)
    {
    }
    /// <summary>
    /// 查询分页数据记录总数
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>返回记录总数</returns>
    public abstract long GetTotalCount(PageRequest request);
}