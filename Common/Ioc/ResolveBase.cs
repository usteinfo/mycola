namespace MyCloa.Common.Ioc;

/// <summary>
/// 对象创建抽象类
/// </summary>
public abstract class ResolveBase : IResolve
{
    /// <summary>
    /// 命令创建服务对象
    /// </summary>
    private readonly IResolveCommandService _resolveCommandService;
    /// <summary>
    /// 构造函数
    /// </summary>
    /// <param name="resolveCommandService">命令创建服务对象，如果为null,使用：ResolveCommandService.Instance</param>
    protected ResolveBase(IResolveCommandService resolveCommandService)
    {
        _resolveCommandService = resolveCommandService ?? ResolveCommandService.Instance;
    }
    /// <summary>
    /// 创建类型为T的对象
    /// </summary>
    /// <typeparam name="T">待创建对象类型</typeparam>
    /// <returns>返回创建成功的对象</returns>
    public abstract T Resolve<T>();

    /// <summary>
    /// 创建命令对象
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <returns>返回创建成功的命令对象</returns>
    public ICommand Resolve(string name)
    {
        ICommand command = CreateCommand(name);
        command.Resolve = this;
        return command;
    }

    /// <summary>
    /// 创建命令虚拟方法
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <returns>返回创建成功的命令对象</returns>
    protected virtual ICommand CreateCommand(string name)
    {
        return _resolveCommandService.Resolve(name);
    }
}