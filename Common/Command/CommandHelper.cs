// using System.Reflection;
//
// namespace MyCloa.Common.Command;
//
// /// <summary>
// /// 命令帮助类
// /// </summary>
// public sealed class CommandHelper:ICommandHelper
// {
//     private static ICommandHelper DefaultInstance = new CommandHelper();
//     public static ICommandHelper GetCommandHelper(ICommandHelper commandHelper = null)
//     {
//         if (commandHelper != null)
//         {
//             return commandHelper;
//         }
//         return DefaultInstance;
//     }
//     private ICommandHelper proxy = null;
//
//     public CommandHelper()
//     {
//         proxy = new DefaultCommandHelper();
//     }
//     public CommandHelper(ICommandHelper commandHelper)
//     {
//         proxy = commandHelper;
//     }
//
//     /// <summary>
//     /// 验证命令是否存在
//     /// </summary>
//     /// <param name="commandName">命令名称</param>
//     /// <returns></returns>
//     public bool CommandValid(string commandName)
//     {
//         return proxy.CommandValid(commandName);
//     }
//
//     /// <summary>
//     /// 验证命令是否存在
//     /// </summary>
//     /// <param name="commandName">命令名称</param>
//     /// <returns></returns>
//     public bool ValidAuthentication(string commandName)
//     {
//         return proxy.ValidAuthentication(commandName);
//     }
//
//     /// <summary>
//     /// 验证命令是否需要授权
//     /// </summary>
//     /// <param name="commandName">命令名称</param>
//     /// <returns></returns>
//     public bool ValidAuthorization(string commandName)
//     {
//         return proxy.ValidAuthorization(commandName);
//     }
//
//     /// <summary>
//     /// 验证命令是否需要认证
//     /// </summary>
//     /// <param name="commandName">命令名称</param>
//     /// <typeparam name="TCommandData"></typeparam>
//     /// <returns></returns>
//     public TCommandData GetCommandData<TCommandData>(string commandName) where TCommandData : CommandData
//     {
//         return proxy.GetCommandData<TCommandData>(commandName);
//     }
//
//     /// <summary>
//     /// 扫描类并按类型进行注册
//     /// </summary>
//     /// <param name="assemblies">需要扫描的程序集</param>
//     /// <param name="action">命令特性数据处理</param>
//     public void ScanCommand<TCommand, TCommandData>(IEnumerable<Assembly> assemblies,
//         Func<TCommand, TCommandData, TCommandData>? action = null)
//         where TCommand : CommandAttribute
//         where TCommandData : CommandData, new()
//     {
//         proxy.ScanCommand(assemblies, action);
//     }
// }