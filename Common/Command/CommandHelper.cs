using System.Reflection;
using MyCloa.Common.Ioc;

namespace MyCloa.Common.Command;

/// <summary>
/// 命令帮助类
/// </summary>
public static class CommandHelper
{
    /// <summary>
    /// 接口字典
    /// </summary>
    private static readonly Dictionary<string, CommandData> CommandDictoryDictionary =
        new Dictionary<string, CommandData>();

    /// <summary>
    /// 初始化是否完成
    /// </summary>
    private static bool _isInit = false;

    private static object _lockobject = new object();

    /// <summary>
    /// 验证命令是否存在
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns></returns>
    internal static bool CommandValid(string commandName)
    {
        return CommandDictoryDictionary.ContainsKey(commandName);
    }

    /// <summary>
    /// 验证命令是否存在
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns></returns>
    internal static bool ValidAuthentication(string commandName)
    {
        if (CommandDictoryDictionary.TryGetValue(commandName, out var value))
        {
            return value.RequestAuthentication;
        }

        return false;
    }

    /// <summary>
    /// 验证命令是否需要授权
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns></returns>
    internal static bool ValidAuthorization(string commandName)
    {
        if (CommandDictoryDictionary.TryGetValue(commandName, out var value))
        {
            return value.RequestAuthorization;
        }

        return false;
    }

    /// <summary>
    /// 验证命令是否需要认证
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <typeparam name="TCommandData"></typeparam>
    /// <returns></returns>
    internal static TCommandData GetCommandData<TCommandData>(string commandName) where TCommandData : CommandData
    {
        if (CommandDictoryDictionary.TryGetValue(commandName, out var value))
        {
            return (TCommandData)value;
        }

        return default(TCommandData);
    }

    /// <summary>
    /// 扫描类并按类型进行注册
    /// </summary>
    /// <param name="assemblies">需要扫描的程序集</param>
    /// <param name="action">命令特性数据处理</param>
    public static void ScanCommand<TCommand, TCommandData>(IEnumerable<Assembly> assemblies,
        Func<TCommand, TCommandData, TCommandData>? action = null)
        where TCommand : CommandAttribute
        where TCommandData : CommandData, new()
    {
        CheckIsInit<TCommand, TCommandData>();

        lock (_lockobject)
        {
            CheckIsInit<TCommand, TCommandData>();
            foreach (var assembly in assemblies)
            {
                ScanCommand(assembly, action);
            }

            _isInit = true;
        }
    }

    /// <summary>
    /// 检查命令扫描是否已初始化
    /// </summary>
    /// <typeparam name="TCommand">命令属性类型</typeparam>
    /// <typeparam name="TCommandData">命令属性数据</typeparam>
    /// <exception cref="BusinessException"></exception>
    private static void CheckIsInit<TCommand, TCommandData>() where TCommand : CommandAttribute
        where TCommandData : CommandData, new()
    {
        if (_isInit)
        {
            throw BusinessException.Create("命令注册不能重复初始化。");
        }
    }

    /// <summary>
    /// 扫描类并按类型进行注册
    /// </summary>
    /// <param name="assembly">需要扫描的程序集</param>
    /// <param name="action">命令特性数据处理</param>
    private static void ScanCommand<TCommand, TCommandData>(Assembly assembly,
        Func<TCommand, TCommandData, TCommandData>? action = null) where TCommand : CommandAttribute
        where TCommandData : CommandData, new()
    {
        var types = assembly.GetTypes();
        var commandType = typeof(ICommand);
        var resolveService = ResolveCommandService.Instance;
        foreach (var type in types)
        {
            if (!type.IsClass)
            {
                continue;
            }

            if (type.GetInterface(commandType.FullName) == null)
            {
                continue;
            }

            var attribute = type.GetCustomAttribute<TCommand>();
            if (attribute == null)
            {
                continue;
            }

            resolveService.Register(attribute.Name, type);
            TCommandData commandData = new TCommandData
            {
                CommandType = type,
                RequestAuthentication = attribute.RequestAuthentication,
                RequestAuthorization = attribute.RequestAuthorization
            };
            if (action != null)
            {
                commandData = action(attribute, commandData);
            }

            CommandDictoryDictionary.Add(attribute.Name, commandData);
        }
    }
}