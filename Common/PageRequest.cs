namespace MyCloa.Common;

/// <summary>
/// 分页请求参数
/// </summary>
/// <typeparam name="TQueryParamer">分页扩展参数</typeparam>
public class PageRequest<TQueryParamer> : PageRequest
{
    /// <summary>
    /// 扩展参数
    /// </summary>
    public TQueryParamer QueryParamer { get; set; }
}
/// <summary>
/// 分页请求参数
/// </summary>
public class PageRequest
{
    /// <summary>
    /// 请求分页大小
    /// </summary>
    public int PageSize { get; set; }
    /// <summary>
    /// 页面
    /// </summary>
    public int PageIndex { get; set; }
    /// <summary>
    /// 是否包括汇总数据
    /// </summary>
    public bool IncludeSummary { get; set; }
    /// <summary>
    /// 检验分页参数
    /// </summary>
    /// <returns></returns>
    public virtual bool Valid()
    {
        if (PageIndex < 0)
        {
            ErrorMessage = "PageIndex不能小于0";
            return false;
        }

        if (PageSize <= 0)
        {
            ErrorMessage = "PageSize不能小于等于0";
            return false;
        }

        if (PageSize > _maxPageSize)
        {
            ErrorMessage = $"PageSize大于最大值:{_maxPageSize}";
            return false;
        }

        return true;
    }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string ErrorMessage { get; protected set; } = "";
    private static readonly long DefaultMaxPageSize = 1000;
    private static long _maxPageSize = DefaultMaxPageSize;

    /// <summary>
    /// 分页最大值
    /// </summary>
    /// <exception cref="BusinessException">不能小于0</exception>
    public static long MaxPageSize
    {
        get
        {
            return _maxPageSize;
        }
        set
        {
            if (value <= 0)
            {
                throw new BusinessException("PageSize不能小于等于0");
            }
            _maxPageSize = value;
        }
    }
}