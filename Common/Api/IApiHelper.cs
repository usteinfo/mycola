using MyCloa.Common.Command;

namespace MyCloa.Common.Api;

/// <summary>
/// Api网关接口
/// </summary>
public interface IApiHelper
{
    /// <summary>
    /// 执行命令
    /// </summary>
    /// <param name="requestStringEntity">命令输入参数</param>
    /// <typeparam name="TCommandData">命令类型数据</typeparam>
    /// <returns>返回执行后的json字符串</returns>
    Task<string> Call<TCommandData>(RequestStringEntity requestStringEntity) where TCommandData : CommandData;
    /// <summary>
    /// 调用远程命令
    /// </summary>
    /// <param name="requestStringEntity">命令输入参数</param>
    /// <returns>返回执行后的json字符串</returns>
   Task<string> CallRemote(RequestStringEntity requestStringEntity);
}