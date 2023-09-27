using System.Diagnostics;

namespace MyCloa.Common.Ioc;

/// <summary>
/// 命令注册和创建服务实现，默认实现类
/// </summary>
internal sealed class ResolveCommandService : IResolveCommandService
{
    private static readonly IResolveCommandService DefaultResolveCommandService = new ResolveCommandService();
    private static IResolveCommandService _resolveCommandService = null;
    public static IResolveCommandService Instance
    {
        get { return _resolveCommandService ?? DefaultResolveCommandService; }
    }

    /// <summary>
    /// 设置默认ResolveCommandService实例
    /// </summary>
    /// <param name="resolveCommandService">命令注册和创建对象实例</param>
    public static void SetResolveCommandService(IResolveCommandService resolveCommandService)
    {
        lock (typeof(ResolveCommandService))
        {
            if(_resolveCommandService != null)
            {
                throw BusinessException.Create("ResolveCommandService只能设置一次。");
            }

            _resolveCommandService = resolveCommandService;
        }
    } 
    private ResolveCommandService(){}
    /// <summary>
    /// 命令和命令对象类型字典
    /// </summary>
    private readonly Dictionary<string, Type> _dictionary = new Dictionary<string, Type>();
    /// <summary>
    /// 注册命令服务，到ioc容器
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <param name="targetType">命令类型</param>
    public void Register(string name, Type targetType)
    {
        _dictionary.Add(name,targetType);
    }
    /// <summary>
    /// 从IOC容器获取服务
    /// </summary>
    /// <param name="name">命令名称</param>
    /// <returns>返回命令对象</returns>
    public ICommand Resolve(string name)
    {
        if (!_dictionary.ContainsKey(name))
        {
           throw BusinessException.Create("命令不存在：{0}", new []{name});
        }
        var  targetType = _dictionary[name];
        Debug.Assert(targetType.FullName != null, "targetType.FullName != null");
        var command = targetType.Assembly.CreateInstance(targetType.FullName, true);
        if(command == null)
        {
           throw BusinessException.Create("命令创建失败：{0}", new object[] { name });
        }
        return (ICommand)command;
    }
}