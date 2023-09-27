namespace MyCloa.Common.Api;

/// <summary>
/// 命令服务初始化参数类型
/// </summary>
public class ApiHelpOption
{
    /// <summary>
    /// 参数校验类型实现
    /// </summary>
    public Type? ValidReqtuestType { get; set; }

    /// <summary>
    /// IOC容器实现
    /// </summary>
    public Type ResolveType { get; set; }

    /// <summary>
    /// 认证接口实现
    /// </summary>
    public Type? AuthenticationType { get; set; }

    /// <summary>
    /// 授权接口实现
    /// </summary>
    public Type? AuthorizationType { get; set; }

    /// <summary>
    /// 远程命令类型
    /// </summary>
    public Type? RemoteCommandType { get; set; }

    /// <summary>
    /// 数据序列化类型
    /// </summary>
    public Type? DataSerializerType { get; set; }

    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServerName { get; set; } = "";
}