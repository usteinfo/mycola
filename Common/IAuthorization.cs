using MyCloa.Common.Command;

namespace MyCloa.Common;

/// <summary>
/// 授权接口
/// </summary>
public interface IAuthorization
{
    /// <summary>
    /// 验证是否授权通过
    /// </summary>
    /// <param name="requestEntity">请求对象</param>
    /// <param name="commandData">命令特性数据</param>
    /// <returns>true 通过 false 失败</returns>
    bool IsAuthorization(RequestEntity requestEntity, CommandData commandData);
}