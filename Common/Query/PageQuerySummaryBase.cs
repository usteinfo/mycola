namespace MyCloa.Common.Query;

/// <summary>
/// 带汇总信息的分页数据查询
/// </summary>
/// <typeparam name="TResult">返回结果类型</typeparam>
/// <typeparam name="TData">返回数据集类型</typeparam>
/// <typeparam name="TSummary">返回汇总数据集类型</typeparam>
/// <typeparam name="TQueryParamer"></typeparam>
public abstract class
    PageQuerySummaryBase<TQueryParamer, TResult, TData, TSummary> : PageQuerySummaryInnerBase<PageRequest<TQueryParamer>
        , TResult, TData, TSummary>
    where TResult : PageResult<TData, TSummary>
{
}

/// <summary>
/// 带汇总信息的分页数据查询
/// </summary>
/// <typeparam name="TResult">返回结果类型</typeparam>
/// <typeparam name="TData">返回数据集类型</typeparam>
/// <typeparam name="TSummary">返回汇总数据集类型</typeparam>
public abstract class
    PageQuerySummaryBase<TResult, TData, TSummary> : PageQuerySummaryInnerBase<PageRequest, TResult, TData, TSummary>
    where TResult : PageResult<TData, TSummary>
{
}