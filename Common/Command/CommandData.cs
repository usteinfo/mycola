namespace MyCloa.Common.Command;

/// <summary>
/// 命令信息
/// 可扩展此类，实际更多信息，比如增加权限验证相关数据
/// 与命令属性一一对应
/// </summary>
public class CommandData
{
    /// <summary>
    /// 命令类型
    /// </summary>
    public Type CommandType { get; internal set; }
    /// <summary>
    /// 接口是否启用用户认证
    /// </summary>
    public bool RequestAuthentication { get; internal set; }
    
    /// <summary>
    /// 接口请求授权
    /// </summary>
    public  bool RequestAuthorization { get; internal set; }
}