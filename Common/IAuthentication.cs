namespace MyCloa.Common;

/// <summary>
/// 认证接口
/// </summary>
public interface IAuthentication
{
    /// <summary>
    /// 验证是否认证通过
    /// </summary>
    /// <param name="requestEntity">请求对象</param>
    /// <returns>true 通过 false 失败</returns>
    bool IsAuthentication(RequestEntity requestEntity);
}