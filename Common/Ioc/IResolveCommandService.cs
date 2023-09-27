namespace MyCloa.Common.Ioc;

/// <summary>
/// 命令服务接口，提供创建命令和注册命令
/// </summary>
public interface IResolveCommandService
{
    /// <summary>
    /// 注册命令服务，到ioc容器
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <param name="targetType">命令类型</param>
    void Register(string name,Type targetType);
    /// <summary>
    /// 从IOC容器获取服务
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <returns>返回命令对象</returns>
    ICommand Resolve(string name);
}