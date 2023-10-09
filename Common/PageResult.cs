using System.Collections.ObjectModel;

namespace MyCloa.Common;

/// <summary>
/// 分页结果对象
/// </summary>
/// <typeparam name="TResult">分页数据明细类型</typeparam>
/// <typeparam name="TSummary">汇总数据类型</typeparam>
public class PageResult<TResult, TSummary> : PageResult<TResult>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageResult()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="request">分页请求对象</param>
    public PageResult(PageRequest request):base(request)
    {
    }
    /// <summary>
    /// 是否包括汇总数据
    /// </summary>
    public bool IncludeSummary { get; set; }
    /// <summary>
    /// 汇总数据
    /// </summary>
    public TSummary? Summary { get; set; }
}

/// <summary>
/// 分页结果数据
/// </summary>
/// <typeparam name="TResult">数据明细类型</typeparam>
public class PageResult<TResult>
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public PageResult()
    {
    }

    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="request">分页请求对象</param>
    public PageResult(PageRequest request)
    {
        this.PageSize = request.PageSize;
        this.PageIndex = request.PageIndex;
    }

    /// <summary>
    /// 分页大小
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 数据所在页
    /// </summary>
    public int PageIndex { get; set; }

    /// <summary>
    /// 记录总数
    /// </summary>
    private long _totalCount = 0;

    /// <summary>
    /// 总页数
    /// </summary>
    public long TotalCount
    {
        get { return _totalCount; }
        set
        {
            _totalCount = value;
            if (PageSize * PageIndex > _totalCount)
            {
                PageIndex = (int)Math.Ceiling((double)TotalCount / PageSize);
            }
        }
    }

    /// <summary>
    /// 分页数据
    /// </summary>
    public IReadOnlyList<TResult> PageData { get; set; } = new ReadOnlyCollection<TResult>(new List<TResult>());
}