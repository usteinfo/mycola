using MyCloa.Common.Ioc;

namespace MyCloa.Common;

/// <summary>
/// 命令接口
/// </summary>
public interface ICommand
{
    /// <summary>
    /// IOC容器
    /// </summary>
    IResolve Resolve { get; internal set; }
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="requestStringEntity">命令请求对象</param>
    /// <returns>返回执行结果：json</returns>
    string Run(RequestStringEntity requestStringEntity);
}
