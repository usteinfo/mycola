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
        services.AddTransient(typeof(IRemoteCommand)
            , apiHelpOption.RemoteCommandType ?? typeof(DefaultRemoteCommand));

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
}