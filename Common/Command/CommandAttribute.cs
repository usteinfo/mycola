namespace MyCloa.Common.Command;

/// <summary>
/// 命令特性，用于标注类对象为命令
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class CommandAttribute : Attribute
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public CommandAttribute()
    {
        RequestAuthentication = true;
        RequestAuthorization = true;
        Name = "";
    }
    /// <summary>
    /// 命令名称
    /// 用于注册ioc的名称
    /// 所有名称名称不能重复
    /// </summary>
    public string Name { get; set; }
    
    /// <summary>
    /// 接口是否启用用户认证，默认启用
    /// </summary>
    public bool RequestAuthentication { get; set; }
    
    /// <summary>
    /// 接口请求授权，默认启用
    /// </summary>
    public  bool RequestAuthorization { get; set; }
}