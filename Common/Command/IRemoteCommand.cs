namespace MyCloa.Common.Command;

/// <summary>
/// 远程命令接口
/// </summary>
public interface IRemoteCommand
{
    /// <summary>
    /// 执行远程命令
    /// </summary>
    /// <param name="requestStringEntity">命令请求参数</param>
    /// <returns>返回执行结果</returns>
    Task<string> Execute(RequestStringEntity requestStringEntity);
}