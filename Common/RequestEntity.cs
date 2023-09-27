namespace MyCloa.Common;
/// <summary>
/// 带数据的请求命令
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class RequestEntity<T>:RequestEntity
{
    /// <summary>
    /// 请求数据
    /// </summary>
    public T Data { get; set; }
}
/// <summary>
/// 请求命令
/// </summary>
public class RequestEntity:CommandEntity
{
    /// <summary>
    /// 用户令牌信息
    /// </summary>
    public string Token { get; set; } = "";
    /// <summary>
    /// 请求编号
    /// </summary>
    public string RequestId { get; set; } = "";
}