using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using MyCloa.Common.Command;
using MyCloa.Common.DataSerializer;
using MyCloa.Common.Ioc;
using MyCloa.Common.Valid;

namespace MyCloa.Common.Api;

/// <summary>
/// API服务中间件
/// </summary>
public static class ApiHelperExt
{
    /// <summary>
    /// 注册API服务
    /// </summary>
    /// <param name="services">IOC容器</param>
    /// <param name="apiHelpOption">服务可选项对象</param>
    /// <returns>返回IOC容器对象</returns>
    /// <exception cref="BusinessException"></exception>
    public static IServiceCollection AddApiHelp(this IServiceCollection services, ApiHelpOption apiHelpOption)
    {
        if (apiHelpOption.ResolveType == null)
        {
            throw BusinessException.Create("ioc服务不能为null");
        }

        if (string.IsNullOrEmpty(apiHelpOption.ServerName))
        {
            throw BusinessException.Create("服务名称不能为null");
        }

        ApiHelper.ServerName = apiHelpOption.ServerName;
        services.AddTransient(typeof(IResolve), apiHelpOption.ResolveType);
        services.AddTransient<IApiHelper, ApiHelper>();

        services.AddTransient(typeof(IDataSerializer)
            , apiHelpOption.DataSerializerType ?? typeof(DefaultSerializer));
        services.AddTransient(typeof(IValidRequest)
            , apiHelpOption.ValidReqtuestType ?? typeof(ValidationContextRequest));
        if (apiHelpOption.EnableRemoteCommand)
        {
            services.AddTransient(typeof(IRemoteCommand)
                , apiHelpOption.RemoteCommandType ?? typeof(DefaultRemoteCommand));
        }

        if (apiHelpOption.AuthenticationType != null)
        {
            services.AddTransient(typeof(IAuthentication), apiHelpOption.AuthenticationType);
        }

        if (apiHelpOption.AuthorizationType != null)
        {
            services.AddTransient(typeof(IAuthorization), apiHelpOption.AuthorizationType);
        }

        return services;
    }

    /// <summary>
    /// 扫描命令，支持多个程序集中的命令
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies">待扫描程序集</param>
    /// <param name="action">命令数据处理</param>
    /// <typeparam name="TCommand">命令属性类型</typeparam>
    /// <typeparam name="TCommandData">命令数据类型</typeparam>
    public static void ScanCommand<TCommand, TCommandData>(this IServiceCollection services,
        IEnumerable<Assembly> assemblies,
        Func<TCommand, TCommandData, TCommandData>? action = null)
        where TCommand : CommandAttribute
        where TCommandData : CommandData, new()
    {
        DefaultCommandHelper.Instance.ScanCommand(assemblies,action);
    }

    /// <summary>
    /// 扫描命令，支持多个程序集中的命令
    /// </summary>
    /// <param name="services"></param>
    /// <param name="assemblies">待扫描程序集</param>
    /// <param name="action">命令数据处理</param>
    public static void ScanCommand(this IServiceCollection services, IEnumerable<Assembly> assemblies,
        Func<CommandAttribute, CommandData, CommandData>? action = null)
    {
        ScanCommand<CommandAttribute, CommandData>(services, assemblies, action);
    }
}