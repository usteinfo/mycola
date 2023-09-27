namespace MyCloa.Common.Query;

/// <summary>
/// 分页数据查询，无汇总数据
/// </summary>
/// <typeparam name="TResult">返回结果类型</typeparam>
/// <typeparam name="TData">返回数据集类型</typeparam>
public abstract class PageQueryBase<TResult, TData> : PageQueryInnerBase<PageRequest, TResult, TData>
    where TResult : PageResult<TData>
{
}

/// <summary>
/// 分页数据查询
/// </summary>
/// <typeparam name="TResult">返回结果类型</typeparam>
/// <typeparam name="TData">返回数据集类型</typeparam>
/// <typeparam name="TQueryParamer"></typeparam>
public abstract class
    PageQueryBase<TQueryParamer, TResult, TData> : PageQueryInnerBase<PageRequest<TQueryParamer>, TResult, TData>
    where TResult : PageResult<TData>
{
}