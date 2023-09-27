namespace MyCloa.Common.Valid;

/// <summary>
/// 数据校验
/// </summary>
public interface IValidRequest
{
    /// <summary>
    /// 数据校验
    /// </summary>
    /// <returns>成功返回true,失败返回false</returns>
    ValidResult Valid<T>(T request);
}