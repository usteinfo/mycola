namespace MyCloa.Common.Command;

/// <summary>
/// 置信远程命令调用实现
/// </summary>
public class DefaultRemoteCommand : RemoteCommandBase
{
    /// <summary>
    /// 构造函数，远程路径为：/cloa/invoke
    /// </summary>
    /// <param name="httpClient">httpClient对象</param>
    public DefaultRemoteCommand(HttpClient httpClient) : base(httpClient, "/cloa/invoke")
    {
    }
}