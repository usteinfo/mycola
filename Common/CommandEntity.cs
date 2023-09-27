namespace MyCloa.Common;

/// <summary>
/// 命令实体
/// </summary>
public class CommandEntity
{
    /// <summary>
    /// 命令名称
    /// </summary>
    public string CommandName { get; set; } = "";
    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceName { get; set; } = "";
    
    /// <summary>
    /// 调用远程服务时，需要设置此参数用于切换协议
    /// </summary>
    public bool HttpsSchema { get; set; }
}