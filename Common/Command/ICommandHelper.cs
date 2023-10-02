using System.Reflection;

namespace MyCloa.Common.Command;

public interface ICommandHelper
{
    /// <summary>
    /// 验证命令是否存在
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns></returns>
    bool CommandValid(string commandName);
    /// <summary>
    /// 验证命令是否启用接口认证
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns>返回true,表示需要接口认证</returns>
    bool ValidAuthentication(string commandName);
    /// <summary>
    /// 验证命令是否需要授权
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <returns>需要授权返回true,其他返回 false </returns>
    bool ValidAuthorization(string commandName);
    
    /// <summary>
    /// 扫描类并按类型进行注册
    /// </summary>
    /// <param name="assemblies">需要扫描的程序集</param>
    /// <param name="action">命令特性数据处理</param>
    void ScanCommand<TCommand, TCommandData>(IEnumerable<Assembly> assemblies,
        Func<TCommand, TCommandData, TCommandData>? action = null)
        where TCommand : CommandAttribute
        where TCommandData : CommandData, new();

    /// <summary>
    /// 验证命令是否需要认证
    /// </summary>
    /// <param name="commandName">命令名称</param>
    /// <typeparam name="TCommandData">命令元数据类型</typeparam>
    /// <returns>返回命令元数据，命令不存在时，返回null</returns>
    TCommandData GetCommandData<TCommandData>(string commandName) where TCommandData : CommandData;
}